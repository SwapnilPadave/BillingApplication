using BA.Database;
using BA.Database.Infra;
using BA.Dtos.StudentDto;
using BA.Entities.Student;
using BA.Service.CurrentUserHelper;
using BA.Utility.Result;
using Dapper;

namespace BA.Service.StudentSer
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlCommands _sqlCommands;
        private readonly DapperServiceHelper _dapperServiceHelper;
        private readonly ICurrentUserService _currentUser;
        public StudentService(IUnitOfWork unitOfWork, SqlCommands sqlCommands, DapperServiceHelper dapperServiceHelper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _sqlCommands = sqlCommands;
            _dapperServiceHelper = dapperServiceHelper;
            _currentUser = currentUserService;
        }

        public async Task<Result> AddStudentAsync(AddStudentDetailsDto requestDto, CancellationToken cancellationToken)
        {
            try
            {
                var student = new Student
                {
                    Name = requestDto.Name,
                    Email = requestDto.Email,
                    Address = requestDto.Address,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                await _unitOfWork.StudentRepository.AddAsync(student);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success(student);
            }
            catch (Exception ex)
            {
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error("BA501"));
            }
        }

        public async Task<Result> BulkUploadStudentDetails(string xmlData)
        {
            if (!string.IsNullOrWhiteSpace(xmlData))
            {
                var param = new DynamicParameters();
                param.Add("@XMLData", xmlData);
                param.Add("@CreatedBy", _currentUser.UserId);
                var result = await _dapperServiceHelper.ExecuteStoredProcAsync<int>("USP_InsertBulkUploadStudentsDetails", param);
                if (result == 0)
                {
                    return Result.Failure(new Error("BA1301"));
                }
                return Result.Success();
            }
            return Result.Failure(new Error("BA501"));
        }
    }
}
