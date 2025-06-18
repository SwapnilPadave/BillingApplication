using BA.Database;
using BA.Database.Infra;
using BA.Dtos.NewsPaperDto;
using BA.Entities.NewsPaper;
using BA.Utility.Content;
using BA.Utility.Result;

namespace BA.Service.NewsPaper
{
    public class NewsPaperService : INewsPaperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlCommands _sqlCommands;
        public NewsPaperService(IUnitOfWork unitOfWork
            , SqlCommands sqlCommands)
        {
            _unitOfWork = unitOfWork;
            _sqlCommands = sqlCommands;
        }

        public async Task<Result> GetNewsPapersAsync(CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.NewsPaperRepository.GetAllAsync();
            if (data == null || !data.Any())
            {
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
            }
            return Result.Success(data);
        }

        public async Task<Result> GetNewsPaperByIdAsync(int id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.NewsPaperRepository.GetAsync(id);
            if (data == null)
            {
                return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
            }
            return Result.Success(data);
        }

        public async Task<Result> AddNewsPaperAsync(int userId, AddNewsPaperDto dto, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var newsPaper = new NewsPaperDetails
                {
                    Name = dto.Name,
                    Language = dto.Language,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                var result = await _unitOfWork.NewsPaperRepository.AddAsync(newsPaper);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result> UpdateNewsPaperAsync(int userId, int id, UpdateNewsPaperDto dto, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var newsPaper = await _unitOfWork.NewsPaperRepository.GetAsync(id);
                if (newsPaper == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                newsPaper.Name = dto.Name;
                newsPaper.Language = dto.Language;
                newsPaper.ModifiedBy = userId;
                newsPaper.ModifiedDate = DateTime.Now;
                var result = _unitOfWork.NewsPaperRepository.Update(newsPaper);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ex.Message));
            }
        }

        public async Task<Result> DeleteNewsPaperAsync(int userId, int id, CancellationToken cancellationToken)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var newsPaper = await _unitOfWork.NewsPaperRepository.GetAsync(id);
                if (newsPaper == null)
                {
                    return Result.Failure(new Error(ContentLoader.ReturnLanguageData("BA1001")));
                }
                newsPaper.IsActive = false;
                newsPaper.ModifiedBy = userId;
                newsPaper.ModifiedDate = DateTime.Now;
                var result = _unitOfWork.NewsPaperRepository.Update(newsPaper);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _sqlCommands.ExceptionLogToDatabase(ex);
                return Result.Failure(new Error(ex.Message));
            }
        }
    }
}
