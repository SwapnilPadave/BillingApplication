using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BA.Utility.SqlHelper
{
    public static class SqlServiceHelper
    {
        public static async Task<List<T>> ExecuteStoredProcedureAsync<T>(
            DbContext dbContext,
            string storedProcedureName,
            params SqlParameter[] parameters) where T : class
        {
            var sql = BuildSqlCommand(storedProcedureName, parameters);
            return await dbContext.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }

        public static async Task<int> ExecuteNonQueryStoredProcedureAsync(
            DbContext dbContext,
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            var sql = BuildSqlCommand(storedProcedureName, parameters);
            return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        private static string BuildSqlCommand(string storedProcedureName, SqlParameter[] parameters)
        {
            var paramList = string.Join(", ", parameters.Select(p => p.ParameterName));
            return $"EXEC {storedProcedureName} {paramList}";
        }
    }
}
