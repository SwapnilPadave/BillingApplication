using BA.Dtos.LoginDto;
using BA.Utility.SqlHelper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;

namespace BA.Database
{
    public class SqlCommands
    {
        private readonly BAContext _context;
        public SqlCommands(BAContext context)
        {
            _context = context;
        }

        public async Task ExceptionLogToDatabase(Exception ex)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@Date", DateTime.Now),
                new SqlParameter("@Message", ex?.Message),
                new SqlParameter("@Type", ex?.GetType().ToString()),
                new SqlParameter("@StackTrace", ex?.StackTrace?.ToString()),
                new SqlParameter("@InnerException", ex?.InnerException?.ToString()),
            };

            await SqlServiceHelper.ExecuteNonQueryStoredProcedureAsync(_context, "USP_ExceptionLogToDatabase", sqlParameters);
        }

        public async Task<GetLoginDetails> GetLoginDetails(string userId, string password)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                var result = await connection.QueryFirstOrDefaultAsync<GetLoginDetails>(
                    "USP_GetUserLoginDetails",
                    new { UserId = userId, Password = password },
                    commandType: CommandType.StoredProcedure);

                return result ?? new GetLoginDetails();
            }
        }
    }
}
