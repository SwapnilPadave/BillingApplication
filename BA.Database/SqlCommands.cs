using BA.Utility.SqlHelper;
using Microsoft.Data.SqlClient;

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
                new SqlParameter("@Message", ex.Message),
                new SqlParameter("@Type", ex.GetType().ToString()),
                new SqlParameter("@StackTrace", ex.StackTrace?.ToString()),
                new SqlParameter("@InnerException", ex.InnerException?.ToString()),
            };

            await SqlServiceHelper.ExecuteNonQueryStoredProcedureAsync(_context, "USP_ExceptionLogToDatabase", sqlParameters);
        }
    }
}
