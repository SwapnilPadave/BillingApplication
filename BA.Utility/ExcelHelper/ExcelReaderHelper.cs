using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System.Reflection;

public static class ExcelReaderHelper
{
    public static List<Dictionary<string, string>> ReadExcel(
        IFormFile file,
        int headerRowIndex,
        int dataStartRowIndex)
    {
        var result = new List<Dictionary<string, string>>();

        var extension = Path.GetExtension(file.FileName).ToLower();

        if (extension != ".xlsx")
            throw new Exception("Invalid file format. Only Excel with extension .xlsx files are supported.");

        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            stream.Position = 0;

            using (var doc = SpreadsheetDocument.Open(stream, false))
            {
                var sheet = doc?.WorkbookPart?.Workbook?.Sheets?.GetFirstChild<Sheet>();
                var worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                var allRows = sheetData.Elements<Row>().ToList();

                var headerRow = allRows.FirstOrDefault(r => r.RowIndex == headerRowIndex);
                if (headerRow == null)
                    throw new Exception($"Header row not found at index {headerRowIndex}");

                var headers = headerRow.Elements<Cell>()
                    .Select(c => GetCellValue(doc, c)) // EXACT, no trim/no changes
                    .ToList();

                foreach (var row in allRows.Where(r => r.RowIndex >= dataStartRowIndex))
                {
                    var cells = row.Elements<Cell>().ToList();
                    var rowDict = new Dictionary<string, string>();

                    for (int col = 0; col < headers.Count; col++)
                    {
                        string headerName = headers[col];

                        string cellValue = cells.Count > col
                            ? GetCellValue(doc, cells[col])
                            : "";

                        rowDict[headerName] = cellValue;
                    }

                    result.Add(rowDict);
                }
            }
        }

        return result;
    }

    public static List<T> ReadExcelData<T>(IFormFile file, int headerRow = 1, int dataStartRow = 2) where T : new()
    {
        var result = new List<T>();

        using var stream = new MemoryStream();
        file.CopyTo(stream);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet(1);

        // Read header row
        var headerCells = worksheet.Row(headerRow).CellsUsed();
        var headers = headerCells.ToDictionary(
            c => c.Address.ColumnNumber,
            c => c.GetString().Trim()
        );

        int lastRow = worksheet.LastRowUsed().RowNumber();

        for (int row = dataStartRow; row <= lastRow; row++)
        {
            T obj = new();

            foreach (var header in headers)
            {
                int col = header.Key;
                string headerName = header.Value;

                PropertyInfo? prop = typeof(T).GetProperty(
                    headerName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                );

                if (prop != null)
                {
                    string cellValue = worksheet.Cell(row, col).GetString();

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        object? convertedValue = Convert.ChangeType(cellValue, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                        prop.SetValue(obj, convertedValue);
                    }
                }
            }

            result.Add(obj);
        }

        return result;
    }

    // Read raw cell value
    private static string GetCellValue(SpreadsheetDocument doc, Cell cell)
    {
        if (cell == null || cell.CellValue == null)
            return "";

        string value = cell.CellValue.InnerText;

        if (cell.DataType != null && cell.DataType == CellValues.SharedString)
        {
            var table = doc?.WorkbookPart?.SharedStringTablePart?.SharedStringTable;
            return table.ChildElements[int.Parse(value)].InnerText;
        }

        return value;
    }

    public static string ExportErrorsToExcel<T>(IEnumerable<T> items)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.AddWorksheet("Errors");

        // Get all properties except the List<string> Errors property
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.Name != "Errors")
                             .ToList();

        int col = 1;

        // Write headers for normal properties
        foreach (var p in props)
        {
            var cell = ws.Cell(1, col);
            cell.Value = p.Name;

            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.Yellow;
            cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            col++;
        }

        // Add only ONE custom Errors column
        var errHead = ws.Cell(1, col);
        errHead.Value = "Errors";
        errHead.Style.Font.Bold = true;
        errHead.Style.Fill.BackgroundColor = XLColor.Yellow;
        errHead.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        errHead.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

        int row = 2;

        foreach (var item in items)
        {
            var errorProp = item.GetType().GetProperty("Errors");
            var errList = errorProp?.GetValue(item) as IList<string>;

            if (errList == null || !errList.Any())
                continue;

            col = 1;

            // Write normal properties
            foreach (var p in props)
            {
                var cell = ws.Cell(row, col);
                cell.Value = p.GetValue(item)?.ToString();

                // Apply borders
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                col++;
            }

            // Write joined errors
            var errorCell = ws.Cell(row, col);
            errorCell.Value = string.Join(", ", errList);

            errorCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            errorCell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            row++;
        }

        // Auto-size columns for readability
        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Convert.ToBase64String(stream.ToArray());
    }

}
