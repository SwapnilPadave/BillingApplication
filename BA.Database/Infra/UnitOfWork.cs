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

        public UnitOfWork(BAContext context
            , IUserRepository userRepository
            , IUserLoginMappingRepository userLoginMappingRepository)
        {
            _context = context;
            UserRepository = userRepository;
            UserLoginMappingRepository = userLoginMappingRepository;
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
