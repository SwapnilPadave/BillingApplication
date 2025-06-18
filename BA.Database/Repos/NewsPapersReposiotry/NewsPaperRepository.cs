using BA.Database.Infra;
using BA.Entities.NewsPaper;

namespace BA.Database.Repos.NewsPapersReposiotry
{
    public class NewsPaperRepository : Repository<NewsPaperDetails>, INewsPaperRepository
    {
        public NewsPaperRepository(BAContext context) : base(context)
        {
        }
    }
}
