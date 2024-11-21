using Microsoft.Data.SqlClient;
using Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Tests.Repository.Tests;

public class OrderRepositoryTests
{
    private Order _testOrder = new Order { OrderDate = DateTime.Now, IsShipped = false };
    private Customer _testCustomer = new Customer();

    private SqlConnection _testConnection = new SqlConnection("Data Source = localhost; Initial Catalog = TEST_WebShopSQL; Integrated Security = True; Connect Timeout = 30; Encrypt=True;Trust Server Certificate=True;Application Intent = ReadWrite; Multi Subnet Failover=False");
    private readonly SqlTransaction _transaction;

    [Fact]
    public async Task OrderRepository_AddOverride_ShouldReturnOrderId()
    {
        //Arrange
        var repositoryFake = new OrderRepository(_testConnection, _transaction);

        //Act
        var id = await repositoryFake.Add(_testOrder);
        _testOrder.Id = id;

        //Assert
        Assert.NotEqual(0, id);
    }
    [Fact]
    public async Task OrderRepository_AddCustomerToOrder_ShouldReturnCorrectCustomerId()
    {
        //Arrange
        var customerRepositoryFake = new CustomerRepository(_testConnection, _transaction);
        var orderRepositoryFake = new OrderRepository(_testConnection, _transaction);

        var orderId = await orderRepositoryFake.Add(new Order { OrderDate = DateTime.Now, IsShipped = false });
        var customerId = await customerRepositoryFake.Add(_testCustomer);

        //Act
        await orderRepositoryFake.AddCustomerToOrder(orderId, customerId);


        //Assert
        var customer = await customerRepositoryFake.GetById(customerId);
        var order = await orderRepositoryFake.GetById(orderId);
        order.Customer = customer;
        Assert.Equal(customer.Id, order.Customer.Id);
    }
    [Fact]
    public async Task OrderRepository_UpdateOrderStatus_ShouldReturnTrue()
    {
        //Arrange
        var repositoryFake = new OrderRepository(_testConnection, _transaction);

        var id = await repositoryFake.Add(_testOrder);

        //Act
        await repositoryFake.UpdateOrderStatus(id, true);
        var order = await repositoryFake.GetById(id);

        //Assert
        Assert.True(order.IsShipped);
    }
}
