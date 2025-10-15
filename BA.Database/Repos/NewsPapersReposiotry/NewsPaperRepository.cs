using BA.Database.Infra;
using BA.Dtos.BillDto;
using BA.Dtos.NewsPaperDto;
using BA.Entities.NewsPaper;
using Microsoft.EntityFrameworkCore;

namespace BA.Database.Repos.NewsPapersReposiotry
{
    public class NewsPaperRepository : Repository<NewsPaperDetails>, INewsPaperRepository
    {
        private readonly BAContext _context;
        public NewsPaperRepository(BAContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetNewsPaperDetailsDto>> GetNewPaperListAsync()
        {
            var data = await _context.NewsPaperDetails
                .Where(np => np.IsActive)
                .Select(np => new GetNewsPaperDetailsDto
                {
                    Id = np.Id,
                    Name = np.Name,
                    Language = np.Language,
                    IsActive = np.IsActive
                }).ToListAsync();

            return data;
        }

        public async Task<GetNewsPaperDetailsDto> GetNewsPaperByIdAsync(int id)
        {
            var data = await _context.NewsPaperDetails
                .Where(np => np.Id == id && np.IsActive)
                .Select(np => new GetNewsPaperDetailsDto
                {
                    Id = np.Id,
                    Name = np.Name,
                    Language = np.Language,
                    IsActive = np.IsActive
                }).FirstOrDefaultAsync();

            return data!;
        }

        //public async Task<NewsPaperDetailsDto> GetNewsPaperDetailsByIdAsync(int id)
        //{
        //    var data = await (from n in _context.NewsPaperDetails
        //                      join b in _context.CustomerBillDetails)
        //}
    }
}
