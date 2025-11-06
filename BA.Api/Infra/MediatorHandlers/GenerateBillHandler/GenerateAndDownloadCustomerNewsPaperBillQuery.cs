using BA.Database;
using BA.Dtos.BillDto;
using BA.Entities.Bill;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.Result;
using Dapper;
using MediatR;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace BA.Api.Infra.MediatorHandlers.GenerateBillHandler
{
    public class GenerateAndDownloadCustomerNewsPaperBillQuery : IRequest<Result>
    {
        public int Id { get; set; }

        public class GenerateAndDownloadCustomerNewsPaperBillQueryHandler : IRequestHandler<GenerateAndDownloadCustomerNewsPaperBillQuery, Result>
        {
            private readonly DapperServiceHelper _dapperServiceHelper;
            private readonly ICurrentUserService _currentUser;
            private readonly SqlCommands _sqlCommand;
            public GenerateAndDownloadCustomerNewsPaperBillQueryHandler(DapperServiceHelper dapperServiceHelper, ICurrentUserService currentUser, SqlCommands sqlCommands)
            {
                _dapperServiceHelper = dapperServiceHelper;
                _currentUser = currentUser;
                _sqlCommand = sqlCommands;
            }
            public async Task<Result> Handle(GenerateAndDownloadCustomerNewsPaperBillQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var resultSet = new GetAllBilDetailsWithNewsPaperDto();
                    var param = new DynamicParameters();
                    param.Add("@Id", request.Id);

                    resultSet = await _dapperServiceHelper.QueryMultipleAsync(
                        "Usp_GetCustomerBillDetails", param, multi =>
                        {
                            resultSet.CustomerBillDetails = multi.ReadFirstOrDefault<CustomerBillDetails>() ?? throw new Exception("Bill details not found.");

                            var billDetails = multi.Read<GetNewsPaperBillDetailsForCustomerBillDto>().AsEnumerable();
                            resultSet.BillDetails = billDetails.ToList();

                            return resultSet;
                        });

                    List<NewsPaperDetailsDto> billsDetails = new List<NewsPaperDetailsDto>();

                    if(resultSet.CustomerBillDetails == null)
                    {
                        return Result.Failure(new Error("BA1201"));
                    }

                    if (resultSet.CustomerBillDetails.Id > 0)
                    {
                        foreach (var paperId in resultSet.BillDetails)
                        {
                            var newsP = new NewsPaperDetailsDto()
                            {
                                NewsPaperId = paperId.BillId,
                                NewsPaperName = paperId.NewsPaperName,
                                Quantity = paperId.TotalDays,
                                Price = paperId.TotalAmount
                            };
                            billsDetails.Add(newsP);
                        }
                    }
                    var indianCulture = new CultureInfo("en-IN");
                    var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Margin(40);
                            page.Size(PageSizes.A4);
                            page.PageColor(Colors.Cyan.Lighten5);

                            page.Header()
                            .Text("S.M. PADAVE")
                            .SemiBold().FontSize(30).AlignCenter();
                            page.Content().Column(column =>
                            {
                                column.Item().LineHorizontal(1).LineColor(Colors.Black);

                                column.Item().PaddingBottom(2).Text("B.I.T Chawl no-1, 2nd Floor, Room No-158").FontSize(16).AlignCenter();
                                column.Item().PaddingBottom(2).Text("Love Lane Mazgaon, Byculla,").FontSize(16).AlignCenter();
                                column.Item().PaddingBottom(2).Text("Mumbai No-400010").FontSize(16).AlignCenter();

                                column.Item().PaddingBottom(6).LineHorizontal(1).LineColor(Colors.Black);

                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingBottom(5).Text(text =>
                                    {
                                        text.Span("Mobile No - ");
                                        text.Span("9821565274").SemiBold();
                                    });
                                    row.AutoItem().AlignRight().Text($"Date :- {DateTime.Now.Date.ToString("dd/MM/yyyy")}");
                                });
                                int monthNo = DateTime.Now.Month;
                                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNo);

                                column.Item().PaddingBottom(5).Text($"Bill For The Month Of :- {monthName.ToUpper()}").FontSize(13).AlignCenter();

                                column.Item().PaddingBottom(5).AlignCenter().Row(row =>
                                {
                                    row.RelativeItem().AlignLeft().Text($"From Date :- {resultSet.CustomerBillDetails.FromDate?.ToString("dd/MM/yyyy")}").FontSize(13);
                                    row.RelativeItem().AlignRight().Text($"To Date :- {resultSet.CustomerBillDetails.ToDate?.ToString("dd/MM/yyyy")}").FontSize(13);
                                });

                                column.Item().PaddingBottom(5).Text($"Name :- Mr. {resultSet.CustomerBillDetails.CustomerName}, {resultSet.CustomerBillDetails.CustomerAddress}").FontSize(15).AlignCenter().SemiBold();

                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(50);   // Sr. No.
                                        columns.RelativeColumn(60);   // Paper Name
                                        columns.ConstantColumn(60);   // Copies
                                        columns.ConstantColumn(60);   // Rs.
                                        columns.ConstantColumn(60);   // P.
                                    });

                                    IContainer TableBorder(IContainer container) => container.Padding(0).Border(1).BorderColor(Colors.Black); ;

                                    // Header section for table
                                    table.Header(header =>
                                    {
                                        header.Cell().Element(TableBorder).PaddingLeft(2).Text("Sr. No.").FontSize(12).SemiBold();
                                        header.Cell().Element(TableBorder).PaddingLeft(2).Text("Paper Name").FontSize(12).SemiBold();
                                        header.Cell().Element(TableBorder).PaddingLeft(2).Text("Copies").FontSize(12).SemiBold();
                                        header.Cell().Element(TableBorder).PaddingRight(2).AlignRight().Text("Rs.").FontSize(12).SemiBold();
                                        header.Cell().Element(TableBorder).PaddingRight(2).AlignRight().Text("P.").FontSize(12).SemiBold();
                                    });

                                    // Data for the bill
                                    int srNo = 1;
                                    foreach (var newsPaper in billsDetails)
                                    {
                                        table.Cell().Element(TableBorder).PaddingLeft(2).Text(srNo.ToString()).FontSize(12);
                                        table.Cell().Element(TableBorder).PaddingLeft(2).Text(newsPaper.NewsPaperName).FontSize(12);
                                        table.Cell().Element(TableBorder).PaddingLeft(2).Text(newsPaper.Quantity.ToString()).FontSize(12);

                                        var priceParts = newsPaper.Price.ToString("F2", new CultureInfo("en-IN")).Split('.');
                                        var rupees = priceParts[0];
                                        var paise = priceParts.Length > 1 ? priceParts[1] : "00";

                                        table.Cell().Element(TableBorder).PaddingRight(2).AlignRight().Text("₹ " + string.Format(indianCulture, "{0:N0}", Convert.ToInt32(rupees))).FontSize(12);

                                        table.Cell().Element(TableBorder).PaddingRight(2).AlignRight().Text("." + paise).FontSize(12);
                                        srNo++;
                                    }
                                    decimal totalAmount = 0;
                                    foreach (var t in billsDetails)
                                    {
                                        totalAmount = totalAmount + t.Price;
                                    }
                                    //Service Charge Row
                                    table.Cell().ColumnSpan(2).Element(TableBorder).AlignCenter().Text("Service Charge").FontSize(12).SemiBold();
                                    table.Cell().ColumnSpan(3).Element(TableBorder).AlignRight().PaddingRight(2).Text(resultSet.CustomerBillDetails.ServiceCharge.ToString("C", new CultureInfo("en-IN"))).FontSize(12).SemiBold();
                                    decimal grandTotal = totalAmount + resultSet.CustomerBillDetails.ServiceCharge;

                                    // Total Amount Row
                                    table.Cell().ColumnSpan(2).Element(TableBorder).AlignCenter().Text("Total Amount").FontSize(12).SemiBold();
                                    table.Cell().ColumnSpan(3).Element(TableBorder).AlignRight().PaddingRight(2).Text(grandTotal.ToString("C", new CultureInfo("en-IN"))).FontSize(12).SemiBold();
                                });

                                //Footer section
                                column.Item().PaddingBottom(5).Text("");
                                column.Item().PaddingBottom(4).Text("Paper Will Not Be Delivered if Bill Not Paid Within 1 Month.").FontSize(11).AlignCenter().Bold();
                                column.Item().PaddingBottom(3).LineHorizontal(1).LineColor(Colors.Black);

                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingBottom(5).Text(text =>
                                    {
                                        text.Span("Prepaid By");
                                    });
                                    row.AutoItem().AlignRight().Text("Receiver Signature");
                                });
                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().PaddingBottom(5).Text("S M Padave").AlignLeft().SemiBold();
                                    row.AutoItem().PaddingBottom(5).Text("S.M.Padave").AlignRight().SemiBold();
                                });
                                column.Item().LineHorizontal(1).LineColor(Colors.Black);

                                column.Item().PaddingBottom(4).Text("You can pay your news paper bill amount online.").FontSize(12).AlignCenter();
                                column.Item().PaddingBottom(4).Text("Gpay number :- +919773129135").FontSize(12).AlignCenter();
                                column.Item().PaddingBottom(4).Text("(Note) :- Once payment is done then please share the screenshot on the same number on WhatsApp.").FontSize(11).AlignCenter().Bold();

                                column.Item().LineHorizontal(1).LineColor(Colors.Black);
                            });
                        });
                    });

                    var currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Bill", currentMonth);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var safeName = string.Join("_", resultSet.CustomerBillDetails.CustomerAddress.Split(Path.GetInvalidFileNameChars()));
                    var fileName = $"{resultSet.CustomerBillDetails.Id}_{safeName}_{currentMonth}_{DateTime.Now.Year}.pdf";
                    var newFileName = fileName.Replace(",", "_");

                    string filePath = Path.Combine(folderPath, newFileName);

                    var saveGeneratedFillDetailsParam = new DynamicParameters();
                    saveGeneratedFillDetailsParam.Add("@CustomerId", resultSet.CustomerBillDetails.CustomerId);
                    saveGeneratedFillDetailsParam.Add("@BillId", resultSet.CustomerBillDetails.Id);
                    saveGeneratedFillDetailsParam.Add("@FileName", newFileName);
                    saveGeneratedFillDetailsParam.Add("@FilePath", filePath);
                    saveGeneratedFillDetailsParam.Add("@FileExtension", ".pdf");
                    saveGeneratedFillDetailsParam.Add("@CreatedBy", _currentUser.UserId);

                    await _dapperServiceHelper.ExecuteAsync("Usp_InsertGeneratedBillDetails", saveGeneratedFillDetailsParam);

                    var pdfBytes = document.GeneratePdf();

                    await File.WriteAllBytesAsync(filePath, pdfBytes);
                    string base64String = Convert.ToBase64String(pdfBytes);

                    var response = new GenerateBillDetailsPdfDto()
                    {
                        Base64String = base64String,
                        FileName = newFileName,
                        Extension = ".pdf",
                    };

                    return Result.Success(response);
                }
                catch (Exception ex)
                {
                    await _sqlCommand.ExceptionLogToDatabase(ex);
                    return Result.Failure(new Error(ex.Message));
                }
            }
        }
    }
}