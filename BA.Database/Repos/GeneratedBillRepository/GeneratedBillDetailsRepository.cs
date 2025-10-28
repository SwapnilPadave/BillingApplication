using BA.Database.Infra;
using BA.Entities.GeneratedBill;

namespace BA.Database.Repos.GeneratedBillRepository
{
    public class GeneratedBillDetailsRepository : Repository<GeneratedNewsPaperBillDetails>, IGeneratedBillDetailsRepository
    {
        private readonly BAContext _context;
        public GeneratedBillDetailsRepository(BAContext context) : base(context)
        {
            _context = context;
        }
    }
}
