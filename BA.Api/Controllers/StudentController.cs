using AutoMapper;
using BA.Api.Infra.Model;
using BA.Api.Infra.Requests.StudentRequest;
using BA.Api.Infra.Validators;
using BA.Dtos.StudentDto;
using BA.Service.StudentSer;
using BA.Utility.ExcelHelper;
using BA.Utility.XmlHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BA.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        public StudentController(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        [HttpPost("Add")]
        public async Task<ResponseModel> AddStudent([FromBody] AddStudentRequest requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<AddStudentDetailsDto>(requestDto);
            var result = await _studentService.AddStudentAsync(request, cancellationToken);
            if (result.IsSuccess)
            {
                return APISuccessResponse("BA100", result.Data!);
            }
            return APIFailureResponse(result.Error.ErrorMsg, null!);
        }

        [HttpPost("Upload")]
        public async Task<ResponseModel> BulkUpload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return APIFailureResponse("BA513", null!);
                }
                var studentList = ExcelReaderHelper.ReadExcelData<GetStudentsDetailsForBulkUploadDto>(file, 1, 3);

                var totalRowsCount = studentList.Count;

                var validator = new StudentBulkUploadValidator();
                var hasErrors = ValidationHelper.ValidateList(studentList, validator);

                if (hasErrors.hasError)
                {
                    var excelBytes = ExcelReaderHelper.ExportErrorsToExcel(studentList);
                    return APIFailureResponse("BA1301", new { base64Data = excelBytes, TotalRows = totalRowsCount, ErrorRows = hasErrors.errorRowCount, RowsUploaded = 0 });
                }
                else
                {
                    var xmlData = XmlReaderHelper.ToXml(studentList, "Students", "Student");
                    var result = await _studentService.BulkUploadStudentDetails(xmlData);
                    if (result.IsSuccess)
                    {
                        return APISuccessResponse("BA100", new { base64Data = string.Empty, TotalRows = totalRowsCount, ErrorRows = hasErrors.errorRowCount, RowsUploaded = totalRowsCount });
                    }
                    return APIFailureResponse(result.Error.ErrorMsg, null!);
                }
            }
            catch (Exception ex)
            {
                return APIFailureResponse(ex.Message, null!);
            }
        }
    }
}
