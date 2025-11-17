using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BA.Database
{
    public class DapperServiceHelper
    {
        private readonly BAContext _context;

        public DapperServiceHelper(BAContext context)
        {
            _context = context;
        }

        public async Task<int> ExecuteAsync(string storedProcedure, DynamicParameters parameters)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<T>> QueryListAsync<T>(string storedProcedure, DynamicParameters? parameters = null)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            var result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<T?> QuerySingleAsync<T>(string storedProcedure, DynamicParameters parameters)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());

            return await connection.QuerySingleOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<TResult> QueryMultipleAsync<TResult>(string storedProcedure, DynamicParameters parameters, Func<SqlMapper.GridReader, TResult> map)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            using var multi = await connection.QueryMultipleAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);

            return map(multi);
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, DynamicParameters? parameters = null)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<T> ExecuteStoredProcAsync<T>(string storedProcedure, DynamicParameters parameters)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());

            var result =  await connection.QueryFirstOrDefaultAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return result!;
        }
    }
}
