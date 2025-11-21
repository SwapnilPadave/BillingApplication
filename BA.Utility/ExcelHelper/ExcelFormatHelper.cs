using ClosedXML.Excel;

namespace BA.Utility.ExcelHelper
{
    public static class ExcelFormatHelper
    {
        public static void ApplyCurrencyFormat(IXLWorksheet sheet, int rowNumber, int columnNumber, decimal? value)
        {
            var cell = sheet.Cell(rowNumber, columnNumber);
            cell.Value = value;
            cell.Style.NumberFormat.Format = "₹ #,##0.00";
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        public static void ApplyDateFormat(IXLWorksheet sheet, int rowNumber, int columnNumber, string? value)
        {
            var cell = sheet.Cell(rowNumber, columnNumber);

            if (DateTime.TryParse(value, out DateTime dt))
            {
                cell.Value = dt;
                cell.Style.DateFormat.Format = "dd-MM-yyyy";
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }
            else
            {
                cell.Value = value;
                cell.Style.DateFormat.Format = "dd-MM-yyyy";
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }
        }

        public static void ApplyIntegerFormat(IXLWorksheet sheet, int rowNumber, int columnNumber, int? value)
        {
            var cell = sheet.Cell(rowNumber, columnNumber);
            cell.Value = value;
            cell.Style.NumberFormat.Format = "#,##0";
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        public static void ApplyStringFormat(IXLWorksheet sheet, int rowNumber, int columnNumber, string? value)
        {
            var cell = sheet.Cell(rowNumber, columnNumber);
            cell.Value = value;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        public static void ApplyHeaderFormat(IXLWorksheet sheet, int rowNumber, int columnNumber, string? value)
        {
            var cell = sheet.Cell(rowNumber, columnNumber);
            cell.Value = value;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            cell.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
        }

        public static void MergeCells(IXLWorksheet sheet, int rowNumber, int fromColumn, int toColumn, string value = "")
        {
            var range = sheet.Range(rowNumber, fromColumn, rowNumber, toColumn);
            range.Merge();
            if (!string.IsNullOrEmpty(value))
            {
                range.Value = value;
                range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                range.Style.Font.Bold = true;
                range.Style.Fill.BackgroundColor = XLColor.LightGray;
            }
            range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        }
    }
}
