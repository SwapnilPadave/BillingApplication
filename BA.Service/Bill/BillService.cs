using BA.Database;
using BA.Database.Infra;
using BA.Dtos.BillDto;
using BA.Entities.Bill;
using BA.Entities.GeneratedBill;
using BA.Service.Email;
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
        private readonly IEmailService _emailService;
        public BillService(IUnitOfWork unitOfWork
            , SqlCommands sqlCommand
            , IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _sqlCommand = sqlCommand;
            _emailService = emailService;
        }

        public async Task<Result> AddCustomerBillDetails(int userId, AddCustomerBillDetailsDto dto)
        {
            try
            {
                var transaction = await _unitOfWork.BeginTransactionAsync();

                var billDetails = new CustomerBillDetails
                {
                    CustomerId = dto.CustomerId,
                    CustomerName = dto.CustomerName,
                    CustomerAddress = dto.CustomerAddress,
                    FromDate = dto.FromDate,
                    ToDate = dto.ToDate,
                    TotalDays = dto.TotalDays,
                    ServiceCharge = dto.ServiceCharge,
                    TotalAmount = 0,
                    IsBillPaid = false,
                    NewsPaperIds = string.Join(",", dto.NewsPaperIdAndAmount.Select(n => n.Id)),
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                var result = await _unitOfWork.BillRepository.AddAsync(billDetails);
                await _unitOfWork.SaveChangesAsync();

                var newsPaperBillDetails = new List<CustomerNewsPaperBillDetail>();

                foreach (var item in dto.NewsPaperIdAndAmount)
                {
                    var nAmount = CalculateDayWiseTotalAmount(item.NormalDays, item.NormalDayAmount);
                    var sunAmount = CalculateDayWiseTotalAmount(item.SundayDays, item.SundayAmount);
                    var satAmount = CalculateDayWiseTotalAmount(item.SaturdayDays, item.SaturdayAmount);
                    var speAmount = CalculateDayWiseTotalAmount(item.SpecialDays, item.SpecialDayAmount);

                    var paperBillDetails = new CustomerNewsPaperBillDetail
                    {
                        CustomerId = result.CustomerId,
                        BillId = result.Id,
                        NewsPaperId = item.Id,
                        NormalDays = item.NormalDays,
                        Sundays = item.SundayDays,
                        Saturday = item.SaturdayDays,
                        SpecialDays = item.SpecialDays,
                        TotalDays = item.NormalDays + item.SundayDays + item.SaturdayDays + item.SpecialDays,
                        NormalDayAmount = nAmount,
                        SundayAmount = sunAmount,
                        SaturdayAmount = satAmount,
                        SpecialDayAmount = speAmount,
                        TotalAmount = nAmount + sunAmount + satAmount + speAmount,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now
                    };
                    newsPaperBillDetails.Add(paperBillDetails);
                }
                await _unitOfWork.CustomerBillDetailsRepository.AddRangeAsync(newsPaperBillDetails);

                //For update total amount in main table after all calculations
                result.TotalAmount = CalculateTotalAmount(newsPaperBillDetails);
                result.TotalDays = CalculateTotalDays(result.FromDate ?? DateTime.Now, result.ToDate ?? DateTime.Now);
                _unitOfWork.BillRepository.Update(result);
                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();
                return Result.Success(billDetails);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                await _sqlCommand.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> GetAllCustomerBills()
        {
            IEnumerable<CustomerBillDetails>? data = await _unitOfWork.BillRepository.GetAllCustomerBillsAsync();
            if (data == null || !data.Any())
            {
                return Result.Failure(new Error("BA502"));
            }
            return Result.Success(data);
        }

        public async Task<Result> GetCustomerBillById(int id)
        {
            GetCustomerBillDetailsDto? data = await _unitOfWork.BillRepository.GetCustomerBillByIdAsync(id);
            if (data == null)
            {
                return Result.Failure(new Error("BA502"));
            }
            return Result.Success(data);
        }

        public async Task<Result> UpdateCustomerBillDetails(int userId, int id, UpdateCustomerBillDetailsDto dto)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var billDetails = await _unitOfWork.BillRepository.GetAsync(id);
                IEnumerable<CustomerNewsPaperBillDetail>? newsPaperDetails = await _unitOfWork.CustomerBillDetailsRepository.GetAllAsync(x => x.BillId == id);

                if (billDetails == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                billDetails.CustomerId = dto.CustomerId;
                billDetails.CustomerName = dto.CustomerName;
                billDetails.CustomerAddress = dto.CustomerAddress;
                billDetails.FromDate = dto.FromDate;
                billDetails.ToDate = dto.ToDate;
                billDetails.TotalDays = dto.TotalDays;
                billDetails.ServiceCharge = dto.ServiceCharge;
                billDetails.TotalAmount = 0;
                billDetails.IsBillPaid = dto.IsBillPaid;
                billDetails.NewsPaperIds = string.Join(",", dto.NewsPaperIdAndAmount.Select(n => n.Id));
                billDetails.ModifiedBy = userId;
                billDetails.ModifiedDate = DateTime.Now;

                var result = _unitOfWork.BillRepository.Update(billDetails);
                await _unitOfWork.SaveChangesAsync();

                foreach (var item in dto.NewsPaperIdAndAmount)
                {
                    var newsPaperBillData = newsPaperDetails.FirstOrDefault(x => x.BillId == id && x.Id == item.Id);
                    if (newsPaperBillData != null)
                    {
                        var nAmount = CalculateDayWiseTotalAmount(item.NormalDays, item.NormalDayAmount);
                        var sunAmount = CalculateDayWiseTotalAmount(item.SundayDays, item.SundayAmount);
                        var satAmount = CalculateDayWiseTotalAmount(item.SaturdayDays, item.SaturdayAmount);
                        var speAmount = CalculateDayWiseTotalAmount(item.SpecialDays, item.SpecialDayAmount);

                        newsPaperBillData.NormalDays = item.NormalDays;
                        newsPaperBillData.Sundays = item.SundayDays;
                        newsPaperBillData.Saturday = item.SaturdayDays;
                        newsPaperBillData.SpecialDays = item.SpecialDays;
                        newsPaperBillData.TotalDays = item.NormalDays + item.SundayDays + item.SaturdayDays + item.SpecialDays;
                        newsPaperBillData.NormalDayAmount = nAmount;
                        newsPaperBillData.SundayAmount = sunAmount;
                        newsPaperBillData.SaturdayAmount = satAmount;
                        newsPaperBillData.SpecialDayAmount = speAmount;
                        newsPaperBillData.TotalAmount = nAmount + sunAmount + satAmount + speAmount;
                        newsPaperBillData.ModifiedBy = userId;
                        newsPaperBillData.ModifiedDate = DateTime.Now;

                        _unitOfWork.CustomerBillDetailsRepository.Update(newsPaperBillData);
                    }
                    else
                    {
                        var nAmount = CalculateDayWiseTotalAmount(item.NormalDays, item.NormalDayAmount);
                        var sunAmount = CalculateDayWiseTotalAmount(item.SundayDays, item.SundayAmount);
                        var satAmount = CalculateDayWiseTotalAmount(item.SaturdayDays, item.SaturdayAmount);
                        var speAmount = CalculateDayWiseTotalAmount(item.SpecialDays, item.SpecialDayAmount);
                        var paperBillDetails = new CustomerNewsPaperBillDetail
                        {
                            CustomerId = billDetails.CustomerId,
                            BillId = billDetails.Id,
                            NewsPaperId = item.Id,
                            NormalDays = item.NormalDays,
                            Sundays = item.SundayDays,
                            Saturday = item.SaturdayDays,
                            SpecialDays = item.SpecialDays,
                            TotalDays = item.NormalDays + item.SundayDays + item.SaturdayDays + item.SpecialDays,
                            NormalDayAmount = nAmount,
                            SundayAmount = sunAmount,
                            SaturdayAmount = satAmount,
                            SpecialDayAmount = speAmount,
                            TotalAmount = nAmount + sunAmount + satAmount + speAmount,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now
                        };
                        await _unitOfWork.CustomerBillDetailsRepository.AddAsync(paperBillDetails);
                    }
                }
                result.TotalAmount = CalculateTotalAmount(newsPaperDetails.ToList());
                result.TotalDays = CalculateTotalDays(result.FromDate ?? DateTime.Now, result.ToDate ?? DateTime.Now);
                _unitOfWork.BillRepository.Update(result);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result.Success(billDetails);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _sqlCommand.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
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
                    return Result.Failure(new Error("BA502"));
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
                return Result.Failure(new Error("BA501"));
            }
        }
        public async Task<Result> UpdateBillStatusAsync(int userId, int id, bool isBillPaid)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var data = await _unitOfWork.BillRepository.GetAsync(id);
                if (data == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                else
                {
                    data.IsBillPaid = isBillPaid;
                    data.ModifiedBy = userId;
                    data.ModifiedDate = DateTime.Now;
                    _unitOfWork.BillRepository.Update(data);
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Result.Success(data);
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                await _sqlCommand.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
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

        public async Task<Result> GenerateBill(int userId, int id)
        {
            try
            {
                CustomerBillDetails? data = await _unitOfWork.BillRepository.GetAsync(id);
                List<NewsPaperDetailsDto> billsDetails = new List<NewsPaperDetailsDto>();
                if (data.Id > 0)
                {
                    var newspaperIds = await _unitOfWork.CustomerBillDetailsRepository.GetBillDetailsAsync(data.Id);
                    foreach (var paperId in newspaperIds)
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
                else
                {
                    return Result.Failure(new Error("BA502"));
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

                            column.Item().PaddingBottom(5).Text($"Name :- Mr. {data.CustomerName}, {data.CustomerAddress}").FontSize(15).AlignCenter().SemiBold();

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

                var safeName = string.Join("_", data.CustomerAddress.Split(Path.GetInvalidFileNameChars()));
                var fileName = $"{data.Id}_{safeName}_{currentMonth}_{DateTime.Now.Year}.pdf";
                var newFileName = fileName.Replace(",", "_");

                string filePath = Path.Combine(folderPath, newFileName);

                var saveDetails = new GeneratedNewsPaperBillDetails()
                {
                    CustomerId = data.CustomerId,
                    BillId = data.Id,
                    FileName = newFileName,
                    FilePath = filePath,
                    FileExtension = ".pdf",
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                await _unitOfWork.GeneratedBillDetailsRepository.AddAsync(saveDetails);
                await _unitOfWork.SaveChangesAsync();

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
                return Result.Failure(new Error("BA501"));
            }
        }
        private int CalculateTotalDays(DateTime fromDate, DateTime toDate)
        {
            int totalDays = (toDate - fromDate).Days + 1;
            return totalDays;
        }
        private decimal CalculateTotalAmount(List<CustomerNewsPaperBillDetail> dto)
        {
            decimal totalAmount = dto.Sum(x => x.SpecialDayAmount + x.SundayAmount + x.SaturdayAmount + x.NormalDayAmount);
            return totalAmount;
        }
        private decimal CalculateDayWiseTotalAmount(int days, decimal amount)
        {
            var totalAmount = days * amount;
            return totalAmount;
        }
    }
}

