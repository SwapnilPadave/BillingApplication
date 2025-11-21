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
        public IEmailTemplateRepository EmailTemplateRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public ICountryRepository CountryRepository { get; }
        public IStateRepository StateRepository { get; }
        public ICityRepository CityRepository { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IPositionRoleRepository PositionRoleRepository { get; }
        public IShiftRepository ShiftRepository { get; }
        public UnitOfWork(BAContext context
            , IUserRepository userRepository
            , IUserLoginMappingRepository userLoginMappingRepository
            , INewsPaperRepository newsPaperRepository
            , ICustomerDetailsRepository customerDetailsRepository
            , IBillRepository billRepository
            , IEmployeeRepository employeeRepository
            , ITokenRepository tokenRepository
            , ICustomerBillDetailsRepository customerBillDetailsRepository
            , IGeneratedBillDetailsRepository generatedBillDetailsRepository
            , IEmailTemplateRepository emailTemplateRepository
            , IStudentRepository studentRepository
            , ICountryRepository countryRepository
            , IStateRepository stateRepository
            , ICityRepository cityRepository
            , IDepartmentRepository departmentRepository
            , IPositionRoleRepository positionRoleRepository
            , IShiftRepository shiftRepository)
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
            EmailTemplateRepository = emailTemplateRepository;
            StudentRepository = studentRepository;
            CountryRepository = countryRepository;
            StateRepository = stateRepository;
            CityRepository = cityRepository;
            DepartmentRepository = departmentRepository;
            PositionRoleRepository = positionRoleRepository;
            ShiftRepository = shiftRepository;
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
