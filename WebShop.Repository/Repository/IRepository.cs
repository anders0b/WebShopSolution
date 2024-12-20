﻿using Dapper;
using System.Data;

namespace Repository.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<int> Add(T entity);
    Task Remove(int id);
    Task Update(T entity);
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
    public virtual async Task<int> Add(T entity)
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

    public virtual async Task<T> GetById(int id)
    {
        var tableName = $"{typeof(T).Name}s";
 
        return await _connectionString.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id }, transaction: _transaction) ?? default!;
    }

    public async Task Remove(int id)
    {
        var tableName = $"{typeof(T).Name}s";

        var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
        await _connectionString.ExecuteAsync(sql, new {Id = id}, transaction: _transaction);
    }

    public virtual async Task Update(T entity)
    {
        var tableName = $"{typeof(T).Name}s";

        var properties = typeof(T).GetProperties().Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)).ToArray();
        var columnNames = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

        var parameterObject = properties.ToDictionary(
        p => p.Name,
        p => p.GetValue(entity));

        var idProperty = typeof(T).GetProperty("Id")!;
        parameterObject["Id"] = idProperty.GetValue(entity);

        var sql = $"UPDATE {tableName} SET {columnNames} WHERE Id = @Id";

        await _connectionString.ExecuteAsync(sql, parameterObject, transaction: _transaction);
    }
}
