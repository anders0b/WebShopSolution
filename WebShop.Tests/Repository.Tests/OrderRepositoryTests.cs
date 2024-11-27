using FakeItEasy;
using Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Tests.Repository.Tests;

public class OrderRepositoryTests
{
    private Order _testOrder = new Order { Id = 1, OrderDate = DateTime.Now, IsShipped = false };
    private Customer _testCustomer = new Customer { Id = 1 };

    [Fact]
    public async Task OrderRepository_AddOverride_ShouldReturnOrderId()
    {
        //Arrange
        var repositoryFake = A.Fake<IOrderRepository>();
        var unitOfWorkFake = A.Fake<IUnitOfWork>();

        A.CallTo(() => unitOfWorkFake.Orders.Add(_testOrder)).Returns(1);

        //Act
        var id = await unitOfWorkFake.Orders.Add(_testOrder);

        //Assert
        Assert.Equal(_testOrder.Id, id);
    }
    [Fact]
    public async Task OrderRepository_AddCustomerToOrder_ShouldReturnCorrectCustomerId()
    {
        //Arrange
        var repositoryFake = A.Fake<IOrderRepository>();
        var unitOfWorkFake = A.Fake<IUnitOfWork>();

        A.CallTo(() => repositoryFake.AddCustomerToOrder(_testOrder.Id, _testCustomer.Id));
        A.CallTo(() => unitOfWorkFake.Orders).Returns(repositoryFake);

        //Act
        await unitOfWorkFake.Orders.AddCustomerToOrder(_testOrder.Id, _testCustomer.Id);

        //Assert
        A.CallTo(() => repositoryFake.AddCustomerToOrder(_testOrder.Id, _testCustomer.Id)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task OrderRepository_UpdateOrderStatus_ShouldReturnTrue()
    {
        //Arrange
        var repositoryFake = A.Fake<IOrderRepository>();
        var unitOfWorkFake = A.Fake<IUnitOfWork>();
        var status = true;

        A.CallTo(() => repositoryFake.UpdateOrderStatus(_testOrder.Id, status)).Invokes(() => _testOrder.IsShipped = true);
        A.CallTo(() => unitOfWorkFake.Orders).Returns(repositoryFake);

        //Act
        await unitOfWorkFake.Orders.UpdateOrderStatus(_testOrder.Id, status);

        //Assert
        A.CallTo(() => repositoryFake.UpdateOrderStatus(_testOrder.Id, status)).MustHaveHappenedOnceExactly();
        Assert.True(_testOrder.IsShipped);
    }
}
