using BA.Database.Repos.UserRepository;
using BA.Database.Repos.UsersRepository;
using Microsoft.EntityFrameworkCore.Storage;

namespace BA.Database.Infra
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IUserLoginMappingRepository UserLoginMappingRepository { get; }
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
