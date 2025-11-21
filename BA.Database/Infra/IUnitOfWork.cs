using BA.Database.Repos.BillRepository;
using BA.Database.Repos.CustomerBillDetailsRepository;
using BA.Database.Repos.CustomerRepository;
using BA.Database.Repos.Depart_Position_StateRepository;
using BA.Database.Repos.EmailTemplateRepository;
using BA.Database.Repos.EmployeeRepository;
using BA.Database.Repos.GeneratedBillRepository;
using BA.Database.Repos.LocationRepository;
using BA.Database.Repos.NewsPapersReposiotry;
using BA.Database.Repos.StudentRepository;
using BA.Database.Repos.TokenRepository;
using BA.Database.Repos.UserRepository;
using BA.Database.Repos.UsersRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace BA.Database.Infra
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IUserLoginMappingRepository UserLoginMappingRepository { get; }
        public INewsPaperRepository NewsPaperRepository { get; }
        public ICustomerDetailsRepository CustomerDetailsRepository { get; }
        public IBillRepository BillRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public ITokenRepository TokenRepository { get; }
        public ICustomerBillDetailsRepository CustomerBillDetailsRepository { get; }
        public IGeneratedBillDetailsRepository GeneratedBillDetailsRepository { get; }
        public IEmailTemplateRepository EmailTemplateRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public ICountryRepository CountryRepository { get; }
        public IStateRepository StateRepository { get; }
        public ICityRepository CityRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IPositionRoleRepository PositionRoleRepository { get; }
        public IShiftRepository ShiftRepository { get; }


        #region // Methods for managing the database context and transactions
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default); 
        #endregion
    }
}
