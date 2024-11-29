using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository.Models;
using WebShop.API.Extensions;
using WebShop.Repository.Models;
using WebShop.Services.Services;

namespace WebShop.Tests.Api.Tests;

public class ProductExtensionTests
{
    private Product _testProduct = new Product { Description = "Test", Name = "Testprodukt", Price = 29.99, Stock = 50 };
    [Fact]
    public async Task AddProduct_ReturnsOkResult_WhenProductIsValid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();

        //Act
        var result = await ProductEndpointExtensions.AddProduct(fakeProductService, _testProduct);

        //Assert
        var resultValue = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Added product Repository.Models.Product", resultValue.Value);
        A.CallTo(() => fakeProductService.AddProduct(_testProduct)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task AddProduct_ReturnsProblemResult_WhenProductIsInvalid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        Product nullProduct = null!;

        //Act
        var result = await ProductEndpointExtensions.AddProduct(fakeProductService, nullProduct);

        //Assert
        var resultValue = Assert.IsType<ProblemHttpResult>(result);
        A.CallTo(() => fakeProductService.AddProduct(nullProduct)).MustNotHaveHappened();
    }
    [Fact]
    public async Task UpdateProduct_ReturnsOkResult_WhenUpdateProductIsValid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();

        var updatedProduct = new Product { Id = 1, Description = "Test", Name = "Testprodukt", Price = 60.99, Stock = 50 };
        A.CallTo(() => fakeProductService.GetProductById(updatedProduct.Id)).Returns(updatedProduct);

        //Act
        var result = await ProductEndpointExtensions.UpdateProduct(fakeProductService, updatedProduct);

        //Assert
        var resultValue = Assert.IsType<Ok<string>>(result);
        A.CallTo(() => fakeProductService.UpdateProduct(updatedProduct)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task UpdateProduct_ReturnsProblem_WhenUpdateProductIsInvalid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();

        var newProduct = new Product();
        A.CallTo(() => fakeProductService.GetProductById(newProduct.Id)).Returns(new Product());

        //Act
        var result = await ProductEndpointExtensions.UpdateProduct(fakeProductService, newProduct);

        //Assert
        var resultValue = Assert.IsType<BadRequest<string>>(result);
        A.CallTo(() => fakeProductService.UpdateProduct(newProduct)).MustNotHaveHappened();
    }
    [Fact]
    public async Task DeleteProduct_ReturnsOkResult_WhenDeleteProductIdIsValid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        int id = 1;
        A.CallTo(() => fakeProductService.GetProductById(id)).Returns(new Product { Id = 1});

        //Act
        var result = await ProductEndpointExtensions.RemoveProduct(fakeProductService, id);

        //Assert
        var resultValue = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Removed product 1", resultValue.Value);
        A.CallTo(() => fakeProductService.RemoveProduct(id)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task DeleteProduct_ReturnsNotFoundResult_WhenDeleteProductIdIsInvalid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        int id = 0;

        //Act
        var result = await ProductEndpointExtensions.RemoveProduct(fakeProductService, id);

        //Assert
        var resultValue = Assert.IsType<BadRequest>(result);
        A.CallTo(() => fakeProductService.RemoveProduct(id)).MustNotHaveHappened();
    }
    [Fact]
    public async Task UpdateStockQuantity_ReturnsOkResult_WhenUpdateStockQuantityIsValid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        int id = 1;
        int stock = 100;
        A.CallTo(() => fakeProductService.GetProductById(id)).Returns(new Product { Id = 1 });

        //Act
        var result = await ProductEndpointExtensions.UpdateStockQuantity(fakeProductService, id, stock);

        //Assert
        var resultValue = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Updated stock quantity for product 1", resultValue.Value);
        A.CallTo(() => fakeProductService.UpdateStockQuantity(id, stock)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task UpdateStockQuantity_ReturnsProblemResult_WhenUpdateStockQuantityIsInvalid()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        int id = 0;
        int stock = 0;

        //Act
        var result = await ProductEndpointExtensions.UpdateStockQuantity(fakeProductService, id, stock);

        //Assert
        var resultValue = Assert.IsType<BadRequest>(result);
        A.CallTo(() => fakeProductService.UpdateStockQuantity(id, stock)).MustNotHaveHappened();
    }
    [Fact]
    public async Task GetAllProducts_ReturnsOkResult()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        var products = new List<Product> { _testProduct };

        A.CallTo(() => fakeProductService.GetAllProducts()).Returns(products);

        //Act
        var result = await ProductEndpointExtensions.GetAllProducts(fakeProductService);

        //Assert
        var resultValue = Assert.IsType<Ok<IEnumerable<Product>>>(result);
        Assert.Equal(products, resultValue.Value);
        A.CallTo(() => fakeProductService.GetAllProducts()).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task GetProductById_ReturnsOkResult_WhenProductExists()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        int id = 1;
        _testProduct.Id = id;

        A.CallTo(() => fakeProductService.GetProductById(id)).Returns(_testProduct);

        //Act
        var result = await ProductEndpointExtensions.GetProductById(fakeProductService, _testProduct.Id);

        //Assert
        var resultValue = Assert.IsType<Ok<Product>>(result);
        Assert.Equal(_testProduct, resultValue.Value);
        A.CallTo(() => fakeProductService.GetProductById(id)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task GetProductById_ReturnsNotFoundResult_WhenProductDoesNotExist()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        int id = 0;

        //Act
        var result = await ProductEndpointExtensions.GetProductById(fakeProductService, id);

        //Assert
        Assert.IsType<BadRequest>(result);
        A.CallTo(() => fakeProductService.GetProductById(id)).MustNotHaveHappened();
    }
    [Fact]
    public async Task GetAllProductsFromOrder_ReturnsOk_ShouldReturnList()
    {
        //Arrange
        var fakeProductService = A.Fake<IProductServices>();
        int orderId = 1;
        var products = new List<OrderItem> { new OrderItem { OrderId = 1, ProductId = 1, Quantity = 5 } };
        A.CallTo(() => fakeProductService.GetAllProductsFromOrder(orderId)).Returns(products);

        //Act
        var result = await ProductEndpointExtensions.GetAllProductsFromOrder(fakeProductService, orderId);

        //Assert
        var resultValue = Assert.IsType<Ok<IEnumerable<OrderItem>>>(result);
        Assert.Equal(products, resultValue.Value);
        A.CallTo(() => fakeProductService.GetAllProductsFromOrder(orderId)).MustHaveHappenedOnceExactly();
    }
}

