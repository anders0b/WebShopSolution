using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repository.Models;
using Repository.Repository;
using System.Data;

namespace WebShop.Repository.Repository;

public interface ICustomerRepository : IRepository<Customer>
{
    Customer GetCustomerByEmail(string email);
    Customer GetCustomerByPhone(string phone);
}
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly IDbConnection _connectionString;
    private string _tableName = "Customers";
    public CustomerRepository(IDbConnection connectionString) : base(connectionString)
    {
        _connectionString = connectionString;
    }
    public Customer GetCustomerByEmail(string email)
    {
        using (var connection = _connectionString)
        {
            return connection.QueryFirstOrDefault<Customer>($"SELECT * FROM {_tableName} WHERE Email = @Email", new { Email = email }) ?? default!;
        }
    }

    public Customer GetCustomerByPhone(string phone)
    {
        using (var connection = _connectionString)
        {
            return connection.QueryFirstOrDefault<Customer>($"SELECT * FROM {_tableName} WHERE Phone = @Phone", new { Phone = phone }) ?? default!;
        }
       
    }
}
