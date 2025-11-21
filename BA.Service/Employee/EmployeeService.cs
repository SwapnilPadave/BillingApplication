using BA.Database;
using BA.Database.Infra;
using BA.Dtos.EmployeeDto;
using BA.Entities.JobApp_Employees;
using BA.Service.CurrentUserHelper;
using BA.Utility.Content;
using BA.Utility.ExcelHelper;
using BA.Utility.Result;
using ClosedXML.Excel;
using Dapper;

namespace BA.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlCommands _sqlCommands;
        private readonly ICurrentUserService _currentUser;
        private readonly DapperServiceHelper _dapper;
        public EmployeeService(IUnitOfWork unitOfWork, SqlCommands sqlCommands, ICurrentUserService currentUser, DapperServiceHelper dapper)
        {
            _unitOfWork = unitOfWork;
            _sqlCommands = sqlCommands;
            _currentUser = currentUser;
            _dapper = dapper;
        }

        public async Task<Result> AddEmployeeDetailsAsync(AddEmployeeDetailsDto request)
        {
            var checks = new Dictionary<Func<Task<bool>>, string>
                {
                    { ()=> _unitOfWork.CountryRepository.AnyAsync(x => x.Id == request.CountryId), "BA1416" },
                    { ()=> _unitOfWork.StateRepository.AnyAsync(x => x.Id == request.StateId), "BA1417" },
                    { ()=> _unitOfWork.CityRepository.AnyAsync(x => x.Id == request.CityId), "BA1418" },
                    { ()=> _unitOfWork.DepartmentRepository.AnyAsync(x => x.Id == request.DepartmentId), "BA1419" },
                    { ()=> _unitOfWork.PositionRoleRepository.AnyAsync(x => x.Id == request.PositionId), "BA1420" },
                    { ()=> _unitOfWork.ShiftRepository.AnyAsync(x => x.Id == request.ShiftId), "BA1421" },
                };
            var errorMessageList = new List<string>();
            foreach (var check in checks)
            {
                if (!await check.Key())
                    errorMessageList.Add(ContentLoader.ReturnLanguageData(check.Value));
            }
            if (errorMessageList.Count > 0)
            {
                return Result.Failure(new Error(string.Join(", ", errorMessageList)));
            }

            try
            {
                var data = new JobApp_EmployeesDetails
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Email = request.Email,
                    MobileNumber = request.MobileNumber,
                    DateOfBirth = request.DateOfBirth,
                    DateOfJoining = request.DateOfJoining,
                    Age = request.Age,
                    Address = request.Address,
                    CountryId = request.CountryId,
                    StateId = request.StateId,
                    CityId = request.CityId,
                    DepartmentId = request.DepartmentId,
                    PositionId = request.PositionId,
                    ShiftId = request.ShiftId,
                    CreatedBy = _currentUser.UserId,
                    CreatedDate = DateTime.Now
                };
                await _unitOfWork.EmployeeRepository.AddAsync(data);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> GetAllEmployeesAsync()
        {
            var data = await _dapper.QueryListAsync<GetEmployeeDto>("Usp_GetEmployeesDetails", null);
            if (data == null || data.Count == 0)
            {
                return Result.Failure(new Error("BA502"));
            }
            return Result.Success(data);
        }

        public async Task<Result> GetEmployeeByIdAsync(int id)
        {
            var param = new DynamicParameters();
            param.Add("@EmployeeId", id);
            var data = await _dapper.QueryFirstOrDefaultAsync<GetEmployeeDto>("Usp_GetEmployeesDetails", param);
            if (data == null)
            {
                return Result.Failure(new Error("BA502"));
            }
            return Result.Success(data);
        }

        public async Task<Result> UpdateEmployeeDetailsAsync(UpdateEmployeeDetailsDto request)
        {
            var errorMessage = await IsErrorMessage(request);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                return Result.Failure(new Error(errorMessage));
            }
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id);
                if (employee != null)
                {
                    employee.FirstName = request.FirstName;
                    employee.MiddleName = request.MiddleName;
                    employee.LastName = request.LastName;
                    employee.Email = request.Email;
                    employee.MobileNumber = request.MobileNumber;
                    employee.DateOfBirth = request.DateOfBirth;
                    employee.DateOfJoining = request.DateOfJoining;
                    employee.Age = request.Age;
                    employee.Address = request.Address;
                    employee.CountryId = request.CountryId;
                    employee.StateId = request.StateId;
                    employee.CityId = request.CityId;
                    employee.DepartmentId = request.DepartmentId;
                    employee.PositionId = request.PositionId;
                    employee.ShiftId = request.ShiftId;
                    employee.ModifiedBy = _currentUser.UserId;
                    employee.ModifiedDate = DateTime.Now;

                    _unitOfWork.EmployeeRepository.Update(employee);
                    await _unitOfWork.SaveChangesAsync();
                    return Result.Success();
                }
                else
                {
                    return Result.Failure(new Error("BA502"));
                }
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> ActivateOrDeactivate(int id, bool isActivate)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (employee == null)
                {
                    return Result.Failure(new Error("BA502"));
                }
                employee.IsActive = isActivate;
                employee.ModifiedBy = _currentUser.UserId;
                employee.ModifiedDate = DateTime.Now;
                _unitOfWork.EmployeeRepository.Update(employee);
                await _unitOfWork.SaveChangesAsync();

                var status = isActivate ? "activated" : "deactivated";
                var replace = new Dictionary<string, string> { { "status", status } };

                return Result.Success(ContentLoader.ReturnLanguageMessage("BA509", replace));
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> ExportToExcelAsync(string? fromDate, string? toDate)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@FromDate", fromDate);
                param.Add("@ToDate", toDate);

                var employeeList = await _dapper.QueryListAsync<GetEmployeeListForExcelReport>("Usp_GetEmployeeListForExport", param);

                using var workbook = new XLWorkbook();
                var ws = workbook.AddWorksheet("EmployeeReport");
                ExcelFormatHelper.MergeCells(ws, 1, 1, 15, "Employee Details Report");
                ExcelFormatHelper.MergeCells(ws, 2, 1, 15);

                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 1, "Full Name");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 2, "Email Address");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 3, "Mobile Number");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 4, "Date Of Birth");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 5, "Date Of Joining");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 6, "Age");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 7, "Address");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 8, "Country Name");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 9, "State Name");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 10, "City Name");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 11, "Department Name");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 12, "Employee Role");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 13, "Shift");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 14, "Shift Time");
                ExcelFormatHelper.ApplyHeaderFormat(ws, 3, 15, "Is Active");


                int row = 4;
                foreach (var item in employeeList)
                {
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 1, item.FullName);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 2, item.Email);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 3, item.MobileNumber);
                    ExcelFormatHelper.ApplyDateFormat(ws, row, 4, item.DateOfBirth);
                    ExcelFormatHelper.ApplyDateFormat(ws, row, 5, item.DateOfJoining);
                    ExcelFormatHelper.ApplyIntegerFormat(ws, row, 6, item.Age);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 7, item.Address);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 8, item.CountryName);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 9, item.StateName);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 10, item.CityName);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 11, item.DeptName);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 12, item.RoleName);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 13, item.ShiftCode);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 14, item.ShiftTime);
                    ExcelFormatHelper.ApplyStringFormat(ws, row, 15, item.IsActive ? "Active" : "Deactive");

                    row++;
                }

                ws.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var base64String = Convert.ToBase64String(stream.ToArray());

                return Result.Success(base64String);
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> BulkUploadEmployeeDetails(string xmlData)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(xmlData))
                {
                    var param = new DynamicParameters();
                    param.Add("@XMLData", xmlData);
                    param.Add("@CreatedBy", _currentUser.UserId);
                    var result = await _dapper.ExecuteStoredProcAsync<int>("USP_InsertBulkUploadEmployeeDetails", param);
                    if (result == 0)
                    {
                        return Result.Failure(new Error("BA1301"));
                    }
                    return Result.Success();
                }
                return Result.Failure(new Error("BA501"));
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }
        private async Task<string> IsErrorMessage(UpdateEmployeeDetailsDto request)
        {
            var errorMessageList = new List<string>();
            if (!await _unitOfWork.CountryRepository.AnyAsync(x => x.Id == request.CountryId))
                errorMessageList.Add(ContentLoader.ReturnLanguageData("BA1416"));
            if (!await _unitOfWork.StateRepository.AnyAsync(x => x.Id == request.StateId))
                errorMessageList.Add(ContentLoader.ReturnLanguageData("BA1417"));
            if (!await _unitOfWork.CityRepository.AnyAsync(x => x.Id == request.CityId))
                errorMessageList.Add(ContentLoader.ReturnLanguageData("BA1418"));
            if (!await _unitOfWork.DepartmentRepository.AnyAsync(x => x.Id == request.DepartmentId))
                errorMessageList.Add(ContentLoader.ReturnLanguageData("BA1419"));
            if (!await _unitOfWork.PositionRoleRepository.AnyAsync(x => x.Id == request.PositionId))
                errorMessageList.Add(ContentLoader.ReturnLanguageData("BA1420"));
            if (!await _unitOfWork.ShiftRepository.AnyAsync(x => x.Id == request.ShiftId))
                errorMessageList.Add(ContentLoader.ReturnLanguageData("BA1421"));

            return string.Join(", ", errorMessageList);
        }
    }
}
