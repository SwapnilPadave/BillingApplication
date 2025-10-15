using BA.Database.Infra;
using BA.Dtos.NewsPaperDto;
using BA.Entities.NewsPaper;

namespace BA.Database.Repos.NewsPapersReposiotry
{
    public interface INewsPaperRepository : IRepository<NewsPaperDetails>
    {
        Task<IEnumerable<GetNewsPaperDetailsDto>> GetNewPaperListAsync();
        Task<GetNewsPaperDetailsDto> GetNewsPaperByIdAsync(int id);
    }
}
