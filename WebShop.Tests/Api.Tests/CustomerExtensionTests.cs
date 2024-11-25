using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository.Models;
using WebShop.API.Extensions;
using WebShop.Services.Services;

namespace WebShop.Tests.Api.Tests;

public class CustomerExtensionTests
{
    private Customer _testCustomer = new();
    [Fact]
    public async Task AddCustomer_ReturnsOkResult()
    {
        //Arrange
        var fakeCustomerService = A.Fake<ICustomerService>();

        //Act
        var result = await CustomerEndpointExtensions.AddCustomer(fakeCustomerService, _testCustomer);

        //Assert
        var resultValue = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Added customer Repository.Models.Customer", resultValue.Value);
        A.CallTo(() => fakeCustomerService.AddCustomer(_testCustomer)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task AddCustomer_ReturnsProblemResult_WhenCustomerIsInvalid()
    {
        //Arrange
        var fakeCustomerService = A.Fake<ICustomerService>();
        Customer nullCustomer = null!;

        //Act
        var result = await CustomerEndpointExtensions.AddCustomer(fakeCustomerService, nullCustomer);

        //Assert
        var resultValue = Assert.IsType<ProblemHttpResult>(result);
        A.CallTo(() => fakeCustomerService.AddCustomer(nullCustomer)).MustNotHaveHappened();
    }
    [Fact]
    public async Task UpdateCustomer_ReturnsOkResult_WhenUpdateCustomerIsValid()
    {
        //Arrange
        var fakeCustomerService = A.Fake<ICustomerService>();

        var updatedCustomer = new Customer
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Testsson"
        };

        //Act
        var result = await CustomerEndpointExtensions.UpdateCustomer(fakeCustomerService, updatedCustomer);

        //Assert
        var resultValue = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Updated customer Repository.Models.Customer", resultValue.Value);
        A.CallTo(() => fakeCustomerService.UpdateCustomer(updatedCustomer)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task UpdateCustomer_ReturnsProblemResult_WhenUpdateCustomerIsInvalid()
    {
        //Arrange
        var fakeCustomerService = A.Fake<ICustomerService>();

        Customer updatedCustomer = null!;

        //Act
        var result = await CustomerEndpointExtensions.UpdateCustomer(fakeCustomerService, updatedCustomer);

        //Assert
        var resultValue = Assert.IsType<ProblemHttpResult>(result);
        A.CallTo(() => fakeCustomerService.UpdateCustomer(updatedCustomer)).MustNotHaveHappened();
    }
    [Fact]
    public async Task DeleteCustomer_ReturnsOkResult_WhenDeleteCustomerdIsValid()
    {
        //Arrange
        var fakeCustomerService = A.Fake<ICustomerService>();
        int id = 1;

        //Act
        var result = await CustomerEndpointExtensions.RemoveCustomer(fakeCustomerService, id);

        //Assert
        var resultValue = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Removed customer 1", resultValue.Value);
        A.CallTo(() => fakeCustomerService.RemoveCustomer(id)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task DeleteCustomer_ReturnsProblemResult_WhenDeleteCustomerdIsInvalid()
    {
        //Arrange
        var fakeCustomerService = A.Fake<ICustomerService>();
        int id = 0;

        //Act
        var result = await CustomerEndpointExtensions.RemoveCustomer(fakeCustomerService, id);

        //Assert
        var resultValue = Assert.IsType<ProblemHttpResult>(result);
        A.CallTo(() => fakeCustomerService.RemoveCustomer(id)).MustNotHaveHappened();
    }
}
        
