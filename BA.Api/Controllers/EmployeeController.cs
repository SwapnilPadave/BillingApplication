using AutoMapper;
using BA.Api.Infra.Model;
using BA.Api.Infra.Requests.CommonRequest;
using BA.Api.Infra.Requests.EmployeeRequest;
using BA.Api.Infra.Validators;
using BA.Dtos.EmployeeDto;
using BA.Dtos.StudentDto;
using BA.Service.Employee;
using BA.Utility.Common_Validator;
using BA.Utility.Enum;
using BA.Utility.ExcelHelper;
using BA.Utility.XmlHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }
        [HttpPost("Add")]
        public async Task<ResponseModel> AddEmployeeDetails([FromBody] AddEmployeeRequest request)
        {
            var dto = _mapper.Map<AddEmployeeDetailsDto>(request);
            var result = await _employeeService.AddEmployeeDetailsAsync(dto);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1422", null!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpGet("GetAll")]
        public async Task<ResponseModel> GetAllEmployees()
        {
            var result = await _employeeService.GetAllEmployeesAsync();
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("GetById")]
        public async Task<ResponseModel> GetEmployeeById(int id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPut("Update")]
        public async Task<ResponseModel> UpdateEmployeeDetails([FromBody] UpdateEmployeeRequest request)
        {
            var dto = _mapper.Map<UpdateEmployeeDetailsDto>(request);
            var result = await _employeeService.UpdateEmployeeDetailsAsync(dto);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA1423", null!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("ActivateOrDeactivate")]
        public async Task<ResponseModel> ActivateOrDeactivate(int id, bool isActivate)
        {
            var result = await _employeeService.ActivateOrDeactivate(id, isActivate);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("ExportToExcel")]
        public async Task<ResponseModel> ExportToExcel([FromQuery] CommonDateFilterRequest request)
        {
            var isBothDateSelected = CommonValidatorMethods.IsBothDatesProvided(request.FromDate, request.ToDate);
            if (isBothDateSelected != "Success.")
                return APIFailureResponse(isBothDateSelected, null!);

            var result = await _employeeService.ExportToExcelAsync(request?.FromDate, request?.ToDate);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA601", new
                {
                    FileName = "EmployeesReport",
                    FileExtension = ".xlsx",
                    Base64Date = result.Data
                });
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);

        }

        [HttpPost("BulkUpload")]
        public async Task<ResponseModel> BulkUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return APIFailureResponse("BA513", null!);
            }

            var employeeList = ExcelReaderHelper.ReadExcelData<EmployeeBulkUploadExcelDto>(file, (int)ExcelEnum.DefaultHeader_StartRow, (int)ExcelEnum.DefaultValues_StartRow);

            var totalRowsCount = employeeList.Count;

            var validator = new EmployeeBulkUploadValidator();
            var hasErrors = ValidationHelper.ValidateList(employeeList, validator);

            if (hasErrors.hasError)
            {
                var excelBytes = ExcelReaderHelper.ExportErrorsToExcel(employeeList);
                return APIFailureResponse("BA1301", new
                {
                    base64Data = excelBytes,
                    TotalRows = totalRowsCount,
                    ErrorRows = hasErrors.errorRowCount,
                    RowsUploaded = 0
                });
            }
            else
            {
                var xmlData = XmlReaderHelper.ToXml(employeeList, "Employees", "Employee");
                var result = await _employeeService.BulkUploadEmployeeDetails(xmlData);
                if (result.IsSuccess)
                {
                    return APISuccessResponse("BA100", new
                    {
                        base64Data = string.Empty,
                        TotalRows = totalRowsCount,
                        ErrorRows = hasErrors.errorRowCount,
                        RowsUploaded = totalRowsCount
                    });
                }
                return APIFailureResponse(result.Error.ErrorMsg, null!);
            }
        }
    }
}
