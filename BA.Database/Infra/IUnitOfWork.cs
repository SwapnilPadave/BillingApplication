using BA.Database.Repos.CustomerRepository;
using BA.Database.Repos.NewsPapersReposiotry;
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

        // Methods for managing the database context and transactions
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
