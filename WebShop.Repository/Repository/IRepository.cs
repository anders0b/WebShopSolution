using Dapper;
using System.Data;

namespace Repository.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<int> Add(T entity);
    Task Remove(T entity);
    Task Update(T entity);
    //void SetTransaction(IDbTransaction transaction);
}
public class Repository<T> : IRepository<T> where T : class
{
    private readonly IDbConnection _connectionString;
    private IDbTransaction _transaction;
    public Repository(IDbConnection connectionString, IDbTransaction transaction)
    {
        _connectionString = connectionString;
        _transaction = transaction;
    }
    //public void SetTransaction(IDbTransaction transaction)
    //{
    //    _transaction = transaction;
    //}
    public async Task<int> Add(T entity)
    {
        var tableName = $"{typeof(T).Name}s";

        var properties = typeof(T).GetProperties()
            .Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

        var columnNames = string.Join(", ", properties.Select(p => p.Name));
        var parametersName = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        var parameterObject = properties.ToDictionary(
            p => p.Name,
            p => p.GetValue(entity));

        var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parametersName}); SELECT CAST(SCOPE_IDENTITY() as int);";

        return await _connectionString.ExecuteScalarAsync<int>(sql, parameterObject, _transaction);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var tableName = $"{typeof(T).Name}s";
        return await _connectionString.QueryAsync<T>($"SELECT * FROM {tableName}", transaction: _transaction);
    }

    public async Task<T> GetById(int id)
    {
        var tableName = $"{typeof(T).Name}s";
 
        return await _connectionString.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id }, transaction: _transaction) ?? default!;
    }

    public async Task Remove(T entity)
    {
        var tableName = $"{typeof(T).Name}s";

        var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
        await _connectionString.ExecuteAsync(sql, entity, transaction: _transaction);

    }

    public async Task Update(T entity)
    {
        var tableName = $"{typeof(T).Name}s";

        var properties = typeof(T).GetProperties();
        var columnNames = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        var sql = $"UPDATE {tableName} SET {columnNames} WHERE Id = @Id";

        await _connectionString.ExecuteAsync(sql, entity, transaction: _transaction);
        
    }
}
