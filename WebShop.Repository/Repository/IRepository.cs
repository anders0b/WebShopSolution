using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebShop.Repository;

namespace Repository.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<int> Add(T entity);
    Task Remove(T entity);
    Task Update(T entity);
}
public class Repository<T> : IRepository<T> where T : class
{
    private readonly IDbConnection _connectionString;
    private readonly IDbTransaction _transaction;
    public Repository(IDbConnection connectionString, IDbTransaction transaction)
    {
        _connectionString = connectionString;
        _transaction = transaction;
    }
    public async Task<int> Add(T entity)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = _connectionString)
        {
            var properties = typeof(T).GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parametersName = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parametersName})";

            return await connection.ExecuteScalarAsync<int>(sql, entity, _transaction);
        }
    }
    public async Task<IEnumerable<T>> GetAll()
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = _connectionString)
        {
            return await connection.QueryAsync<T>($"SELECT * FROM {tableName}", _transaction);
        }
    }

    public async Task<T> GetById(int id)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = _connectionString)
        {
            return await connection.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id }, _transaction) ?? default!;
        }
    }

    public async Task Remove(T entity)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = _connectionString)
        {
            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
            await connection.ExecuteAsync(sql, entity, _transaction);
        }
    }

    public async Task Update(T entity)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = _connectionString)
        {
            var properties = typeof(T).GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            var sql = $"UPDATE {tableName} SET {columnNames} WHERE Id = @Id";

            await connection.ExecuteAsync(sql, entity, _transaction);
        }
    }
}
