namespace ECommerceAdminPanel.Helper;

using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

/// <summary>
/// Dapper Database Helper for executing queries and stored procedures
/// </summary>
public class DapperHelper
{
    private readonly string _connectionString;

    public DapperHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Execute a stored procedure and return a single result
    /// </summary>
    public async Task<T?> QuerySingleOrDefaultAsync<T>(string spName, DynamicParameters? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QuerySingleOrDefaultAsync<T>(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

    /// <summary>
    /// Execute a stored procedure and return multiple results
    /// </summary>
    public async Task<List<T>> QueryAsync<T>(string spName, DynamicParameters? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryAsync<T>(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return result.ToList();
        }
    }

    /// <summary>
    /// Execute a stored procedure that returns multiple result sets
    /// </summary>
    public async Task<SqlMapper.GridReader> QueryMultipleAsync(string spName, DynamicParameters? parameters = null)
    {
        var connection = new SqlConnection(_connectionString);
        return await connection.QueryMultipleAsync(
            spName,
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    /// <summary>
    /// Execute a non-query stored procedure (Insert, Update, Delete)
    /// </summary>
    public async Task<int> ExecuteAsync(string spName, DynamicParameters? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

    /// <summary>
    /// Execute a scalar query that returns a single value
    /// </summary>
    public async Task<object?> ExecuteScalarAsync(string spName, DynamicParameters? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.ExecuteScalarAsync(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

    /// <summary>
    /// Execute a raw SQL query
    /// </summary>
    public async Task<List<T>> QueryRawAsync<T>(string sql, DynamicParameters? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var result = await connection.QueryAsync<T>(sql, parameters);
            return result.ToList();
        }
    }

    /// <summary>
    /// Execute a raw SQL non-query
    /// </summary>
    public async Task<int> ExecuteRawAsync(string sql, DynamicParameters? parameters = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sql, parameters);
        }
    }

    /// <summary>
    /// Get connection string
    /// </summary>
    public string GetConnectionString()
    {
        return _connectionString;
    }
}
