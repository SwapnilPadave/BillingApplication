using BA.Database.Repos.BillRepository;
using BA.Database.Repos.CustomerBillDetailsRepository;
using BA.Database.Repos.CustomerRepository;
using BA.Database.Repos.EmployeeRepository;
using BA.Database.Repos.GeneratedBillRepository;
using BA.Database.Repos.NewsPapersReposiotry;
using BA.Database.Repos.TokenRepository;
using BA.Database.Repos.UserRepository;
using BA.Database.Repos.UsersRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace BA.Database.Infra
{
    public class UnitOfWork : IUnitOfWork
    {
        public BAContext _context;

        public IUserRepository UserRepository { get; }
        public IUserLoginMappingRepository UserLoginMappingRepository { get; }
        public INewsPaperRepository NewsPaperRepository { get; }
        public ICustomerDetailsRepository CustomerDetailsRepository { get; }
        public IBillRepository BillRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public ITokenRepository TokenRepository { get; }
        public ICustomerBillDetailsRepository CustomerBillDetailsRepository { get; }
        public IGeneratedBillDetailsRepository GeneratedBillDetailsRepository { get; }
        public UnitOfWork(BAContext context
            , IUserRepository userRepository
            , IUserLoginMappingRepository userLoginMappingRepository
            , INewsPaperRepository newsPaperRepository
            , ICustomerDetailsRepository customerDetailsRepository
            , IBillRepository billRepository
            , IEmployeeRepository employeeRepository
            , ITokenRepository tokenRepository
            , ICustomerBillDetailsRepository customerBillDetailsRepository
            , IGeneratedBillDetailsRepository generatedBillDetailsRepository)
        {
            _context = context;
            UserRepository = userRepository;
            UserLoginMappingRepository = userLoginMappingRepository;
            NewsPaperRepository = newsPaperRepository;
            CustomerDetailsRepository = customerDetailsRepository;
            BillRepository = billRepository;
            EmployeeRepository = employeeRepository;
            TokenRepository = tokenRepository;
            CustomerBillDetailsRepository = customerBillDetailsRepository;
            GeneratedBillDetailsRepository = generatedBillDetailsRepository;
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return (await _context.SaveChangesAsync(cancellationToken) > 0);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }
    }
}
