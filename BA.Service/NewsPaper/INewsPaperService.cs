using BA.Dtos.NewsPaperDto;
using BA.Utility.Result;

namespace BA.Service.NewsPaper
{
    public interface INewsPaperService
    {
        Task<Result> GetNewsPapersAsync(CancellationToken cancellationToken);
        Task<Result> GetNewsPaperByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result> AddNewsPaperAsync(int userId, AddNewsPaperDto dto, CancellationToken cancellationToken);
        Task<Result> UpdateNewsPaperAsync(int userId, int id, UpdateNewsPaperDto dto, CancellationToken cancellationToken);
        Task<Result> DeleteNewsPaperAsync(int userId, int id, CancellationToken cancellationToken);
    }
}
