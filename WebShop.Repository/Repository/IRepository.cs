using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Repository.Repository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);
}
public class Repository<T> : IRepository<T> where T : class
{
    private readonly string _connectionString;
    public Repository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "null";
    }
    public void Add(T entity)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = new SqlConnection(_connectionString))
        {
            var properties = typeof(T).GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var parametersName = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parametersName})";

            connection.Execute(sql, entity);
        }
    }
    public IEnumerable<T> GetAll()
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.Query<T>($"SELECT * FROM {tableName}");
        }
    }

    public T GetById(int id)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id }) ?? default!;
        }
    }

    public void Remove(T entity)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
            connection.Execute(sql, entity);
        }
    }

    public void Update(T entity)
    {
        var tableName = $"{typeof(T).Name}s";
        using (var connection = new SqlConnection(_connectionString))
        {
            var properties = typeof(T).GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            var sql = $"UPDATE {tableName} SET {columnNames} WHERE Id = @Id";

            connection.Execute(sql, entity);
        }
    }
}
