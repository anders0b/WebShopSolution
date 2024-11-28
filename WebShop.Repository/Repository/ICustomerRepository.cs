using Dapper;
using Repository.Models;
using Repository.Repository;
using System.Data;

namespace WebShop.Repository.Repository;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer> GetCustomerByEmail(string email);
    Task<Customer> GetCustomerByPhone(string phone);
    Task UpdateCustomerEmail(int customerId, string email);
    Task UpdateCustomerPhone(int customerId, string phone);
}

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly IDbConnection _connection;
    private IDbTransaction _transaction;
    private string _tableName = "Customers";
    public CustomerRepository(IDbConnection connection, IDbTransaction transaction) : base(connection, transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }
    public async Task<Customer> GetCustomerByEmail(string email)
    {
        return await _connection.QueryFirstOrDefaultAsync<Customer>($"SELECT * FROM {_tableName} WHERE Email = @Email", new { Email = email }, transaction: _transaction) ?? default!;
    }

    public async Task<Customer> GetCustomerByPhone(string phone)
    {
        return await _connection.QueryFirstOrDefaultAsync<Customer>($"SELECT * FROM {_tableName} WHERE Phone = @Phone", new { Phone = phone }, transaction: _transaction) ?? default!;
    }
    public async Task UpdateCustomerEmail(int customerId, string email)
    {
        var sql = $"UPDATE {_tableName} SET Email = @Email WHERE Id = @CustomerId";
        await _connection.ExecuteAsync(sql, new { Email = email, CustomerId = customerId }, transaction: _transaction);
    }
    public async Task UpdateCustomerPhone(int customerId, string phone)
    {
        var sql = $"UPDATE {_tableName} SET Phone = @Phone WHERE Id = @CustomerId";
        await _connection.ExecuteAsync(sql, new { Phone = phone, CustomerId = customerId }, transaction: _transaction);
    }
}
