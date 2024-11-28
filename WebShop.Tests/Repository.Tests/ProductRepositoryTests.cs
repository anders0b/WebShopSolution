using FakeItEasy;
using Repository.Models;
using Repository.Repository;
using WebShop.Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Tests.Repository.Tests;

public class ProductRepositoryTests()
{
    private Product _testProduct = new Product { Description = "Test", Name = "Testprodukt", Price = 29.99, Stock = 50 };

    [Fact]
    public async Task ProductRepository_UpdateStockQuantity_ShouldReturnPlusOne()
    {
        //Arrange
        var repositoryFake = A.Fake<IProductRepository>();
        var unitOfWorkFake = A.Fake<IUnitOfWork>();

        var id = 1;
        A.CallTo(() => repositoryFake.UpdateStockQuantity(id, 49)).Invokes(() => _testProduct.Stock = 49);
        A.CallTo(() => unitOfWorkFake.Products).Returns(repositoryFake);

        //Act
        await unitOfWorkFake.Products.UpdateStockQuantity(id, 49);

        //Assert
        A.CallTo(() => repositoryFake.UpdateStockQuantity(id, 49)).MustHaveHappenedOnceExactly();
        Assert.Equal(49, _testProduct.Stock);
    }
    [Fact]
    public async Task ProductRepository_GetAllProductsFromOrder_ShouldReturnList()
    {
        //Arrange
        var repositoryFake = A.Fake<IProductRepository>();
        var unitOfWorkFake = A.Fake<IUnitOfWork>();

        var orderId = 1;
        A.CallTo(() => repositoryFake.GetAllProductsFromOrder(orderId)).Returns(new List<OrderItem> { new OrderItem { OrderId = 1, ProductId = 1, Quantity = 50 } });
        A.CallTo(() => unitOfWorkFake.Products).Returns(repositoryFake);

        //Act
        var products = await unitOfWorkFake.Products.GetAllProductsFromOrder(orderId);

        //Assert
        Assert.NotNull(products);
        Assert.Single(products);
        A.CallTo(() => repositoryFake.GetAllProductsFromOrder(orderId)).MustHaveHappenedOnceExactly();
    }
}
