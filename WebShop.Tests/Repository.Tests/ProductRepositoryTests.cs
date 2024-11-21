using Microsoft.Data.SqlClient;
using Repository.Models;
using Repository.Repository;
using WebShop.Repository.Repository;

namespace WebShop.Tests.Repository.Tests;

public class ProductRepositoryTests()
{
    private Product _testProduct = new Product { Description = "Test", Name = "Testprodukt", Price = 29.99, Stock = 50 };
    private SqlConnection _testConnection = new SqlConnection("Data Source = localhost; Initial Catalog = TEST_WebShopSQL; Integrated Security = True; Connect Timeout = 30; Encrypt=True;Trust Server Certificate=True;Application Intent = ReadWrite; Multi Subnet Failover=False");
    private readonly SqlTransaction? _transaction;

    [Fact]
    public async Task ProductRepository_UpdateStockQuantity_ShouldReturnPlusOne()
    {
        //Arrange
        var repositoryFake = new ProductRepository(_testConnection, _transaction);

        var id = await repositoryFake.Add(_testProduct);
        _testProduct.Id = id;

        //Act
        await repositoryFake.UpdateStockQuantity(id, 1);
        var getProduct = await repositoryFake.GetById(id);

        //Assert
        Assert.Equal(_testProduct.Stock - 1, getProduct.Stock);
    }
    [Fact]
    public async Task ProductRepository_GetAllProductsFromOrder_ShouldReturnList()
    {
        //Arrange
        var customerRepositoryFake = new CustomerRepository(_testConnection, _transaction);
        var productRepositoryFake = new ProductRepository(_testConnection, _transaction);
        var orderRepositoryFake = new OrderRepository(_testConnection, _transaction);

        var orderId = await orderRepositoryFake.Add(new Order { OrderDate = DateTime.Now, IsShipped = false });

        var customerId = await customerRepositoryFake.Add(new Customer());

        await orderRepositoryFake.AddCustomerToOrder(orderId, customerId);

        var toAdd = new List<int>();
        var listOfProducts = await productRepositoryFake.GetAll();

        foreach (var product in listOfProducts)
        {
            toAdd.Add(product.Id);
        }

        await orderRepositoryFake.AddProductsToOrder(orderId, toAdd);

        //Act
        var products = await productRepositoryFake.GetAllProductsFromOrder(orderId);

        //Assert
        Assert.NotNull(products);
    }
}
