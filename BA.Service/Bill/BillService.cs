using BA.Database;
using BA.Database.Infra;
using BA.Dtos.BillDto;
using BA.Entities.Bill;
using BA.Utility.Content;
using BA.Utility.Result;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace BA.Service.Bill
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlCommands _sqlCommand;
        public BillService(IUnitOfWork unitOfWork
            , SqlCommands sqlCommand)
        {
            _unitOfWork = unitOfWork;
            _sqlCommand = sqlCommand;
        }

        public async Task<Result> AddCustomerBillDetails(int userId, AddCustomerBillDetailsDto dto)
        {
            try
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();

                var billDetails = new CustomerBillDetails
                {
                    CustomerName = dto.CustomerName,
                    CustomerAddress = dto.CustomerAddress,
                    FromDate = dto.FromDate,
                    ToDate = dto.ToDate,
                    TotalDays = dto.TotalDays,
                    ServiceCharge = dto.ServiceCharge,
                    TotalAmount = CalculateTotalAmount(dto.NewsPaperIdAndAmount),
                    IsBillPaid = false,
                    NewsPaperIds = string.Join(",", dto.NewsPaperIdAndAmount.Select(n => n.Id)),
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _unitOfWork.BillRepository.AddAsync(billDetails);
                await _unitOfWork.SaveChangesAsync();

                var newsPaperBillDetails = new List<CustomerNewsPaperBillDetail>();

                foreach (var item in dto.NewsPaperIdAndAmount)
                {
                    var paperBillDetails = new CustomerNewsPaperBillDetail
                    {
                        CustomerId = result.Id,
                        NewsPaperId = item.Id,
                        NormalDays = item.NormalDays,
                        Sundays = item.SundayDays,
                        Saturday= item.SaturdayDays,
                        SpecialDays = item.SpecialDays,
                        TotalDays = item.NormalDays + item.SundayDays + item.SaturdayDays + item.SpecialDays,
                        NormalDayAmount = item.NormalDayAmount,
                        SundayAmount = item.SundayAmount,
                        SaturdayAmount = item.SaturdayAmount,
                        SpecialDayAmount = item.SpecialDayAmount,
                        TotalAmount = item.NormalDayAmount + item.SundayAmount + item.SaturdayAmount + item.SpecialDayAmount,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow
                    };
                    newsPaperBillDetails.Add(paperBillDetails);
                }
                await _unitOfWork.CustomerBillDetailsRepository.AddRangeAsync(newsPaperBillDetails);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                //Generate PDF Bill and save to folder location.
                await GenerateBill(result.Id);

                return Result.Success(billDetails);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                await _sqlCommand.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }

        public async Task<Result> GetAllCustomerBills()
        {
            var data = await _unitOfWork.BillRepository.GetAllCustomerBillsAsync();
            return Result.Success(data);
        }

        public async Task<Result> GetCustomerBillById(int id)
        {
            var data = await _unitOfWork.BillRepository.GetAsync(id);
            if (data == null)
            {
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
            }
            return Result.Success(data);
        }

        public async Task<Result> UpdateCustomerBillDetails(int userId, int id, UpdateCustomerBillDetailsDto dto)
        {
            try
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                var billDetails = await _unitOfWork.BillRepository.GetAsync(id);
                if (billDetails == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                billDetails.CustomerName = dto.CustomerName;
                billDetails.CustomerAddress = dto.CustomerAddress;
                billDetails.FromDate = dto.FromDate;
                billDetails.ToDate = dto.ToDate;
                billDetails.TotalDays = dto.TotalDays;
                billDetails.ServiceCharge = dto.ServiceCharge;
                billDetails.TotalAmount = CalculateTotalAmount(dto.NewsPaperIdAndAmount);
                billDetails.IsBillPaid = dto.IsBillPaid;
                billDetails.NewsPaperIds = string.Join(",", dto.NewsPaperIdAndAmount.Select(n => n.Id));
                billDetails.ModifiedBy = userId;
                billDetails.ModifiedDate = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result.Success(billDetails);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                await _sqlCommand.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }

        public async Task<Result> DeleteCustomerBill(int userId, int id)
        {
            try
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();
                var billDetails = await _unitOfWork.BillRepository.GetAsync(id);
                if (billDetails == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                billDetails.IsActive = false;
                billDetails.ModifiedBy = userId;

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result.Success(billDetails);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                await _sqlCommand.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA101")));
            }
        }

        private decimal CalculateTotalAmount(List<NewsPaperIdAndAmount> dto)
        {
            decimal totalAmount = dto.Sum(x => x.SpecialDayAmount + x.SundayAmount + x.SaturdayAmount + x.NormalDayAmount);

            return totalAmount;
        }

        public Dictionary<string, int> GetSatAndSunCount(DateTime fromDate, DateTime toDate, int specialDays = 0)
        {
            var weekends = new Dictionary<string, int>();
            int totalDays = (toDate - fromDate).Days + 1;

            for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (weekends.ContainsKey("Saturday"))
                        weekends["Saturday"]++;
                    else
                        weekends["Saturday"] = 1;
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (weekends.ContainsKey("Sunday"))
                        weekends["Sunday"]++;
                    else
                        weekends["Sunday"] = 1;
                }
            }

            int satAndSunCount = weekends.Values.Sum();
            int normalDays = totalDays - satAndSunCount - specialDays;

            weekends.Add("SpecialDays", specialDays);
            weekends.Add("NormalDays", normalDays);

            return weekends;
        }

        public async Task<byte[]> GenerateBill(int id)
        {
            var data = await _unitOfWork.BillRepository.GetAsync(id);
            List<NewsPaperDetailsDto> billsDetails = new List<NewsPaperDetailsDto>();
            if (data.Id > 0)
            {
                var newspaperIds = await _unitOfWork.CustomerBillDetailsRepository.GetBillDetailsAsync(data.Id);
                foreach (var paperId in newspaperIds)
                {
                    var newsP = new NewsPaperDetailsDto()
                    {
                        NewsPaperId = paperId.BillId,
                        NewsPaperName = paperId.BillName,
                        Quantity = data.TotalDays,
                        Price = paperId.Amount
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
                            row.RelativeItem().AlignLeft().Text($"From Date :- {data.FromDate?.ToString("dd/MM/yyyy")}").FontSize(13);
                            row.RelativeItem().AlignRight().Text($"To Date :- {data.ToDate?.ToString("dd/MM/yyyy")}").FontSize(13);
                        });

                        column.Item().PaddingBottom(5).Text($"Name :- Mr. {data.CustomerAddress}").FontSize(15).AlignCenter().SemiBold();

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
                            table.Cell().ColumnSpan(3).Element(TableBorder).AlignRight().PaddingRight(2).Text(data.ServiceCharge.ToString("C", new CultureInfo("en-IN"))).FontSize(12).SemiBold();
                            decimal grandTotal = totalAmount + data.ServiceCharge;

                            // Total Amount Row
                            table.Cell().ColumnSpan(2).Element(TableBorder).AlignCenter().Text("Total Amount").FontSize(12).SemiBold();
                            table.Cell().ColumnSpan(3).Element(TableBorder).AlignRight().PaddingRight(2).Text(grandTotal.ToString("C", new CultureInfo("en-IN"))).FontSize(12).SemiBold();
                        });

                        //Footer section
                        column.Item().PaddingBottom(5).Text("");
                        column.Item().PaddingBottom(2).Text("Paper Will Not Be Delivered if Bill Not Paid Within 1 Month.").FontSize(16).AlignCenter();
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
                    });
                });
            });

            var currentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Bill", currentMonth);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var safeName = string.Join("_", data.CustomerAddress.Split(Path.GetInvalidFileNameChars()));
            var fileName = $"{data.Id}_{safeName}_{currentMonth}_{DateTime.Now.Year}.pdf";

            string filePath = Path.Combine(folderPath, fileName);
            var pdfBytes = document.GeneratePdf();
            await File.WriteAllBytesAsync(filePath, pdfBytes);
            return pdfBytes;
        }
    }
}

