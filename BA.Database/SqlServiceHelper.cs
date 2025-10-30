using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BA.Database
{
    public class SqlServiceHelper
    {
        private readonly BAContext _context;
        public SqlServiceHelper(BAContext context)
        {
            _context = context;
        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(
            string storedProcedureName,
            params SqlParameter[] parameters) where T : class
        {
            var sql = BuildSqlCommand(storedProcedureName, parameters);
            return await _context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

        public async Task<int> ExecuteNonQueryStoredProcedureAsync(
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            var sql = BuildSqlCommand(storedProcedureName, parameters);
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<T?> ExecuteStoredProcedureSingleResultAsync<T>(
            string storedProcedureName,
            params SqlParameter[] parameters) where T : class
        {
            var sql = BuildSqlCommand(storedProcedureName, parameters);
            return await _context.Set<T>().FromSqlRaw(sql, parameters).AsNoTracking().SingleOrDefaultAsync();
        }

        private string BuildSqlCommand(string storedProcedureName, SqlParameter[] parameters)
        {
            var paramList = string.Join(", ", parameters.Select(p => p.ParameterName));
            return $"EXEC {storedProcedureName} {paramList}";
        }
    }
}
