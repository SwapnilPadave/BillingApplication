using BA.Dtos.StudentDto;
using BA.Utility.Result;

namespace BA.Service.StudentSer
{
    public interface IStudentService
    {
        Task<Result> AddStudentAsync(AddStudentDetailsDto requestDto, CancellationToken cancellationToken);
        Task<Result> BulkUploadStudentDetails(string xmlData);
    }
}
