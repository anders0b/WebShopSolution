using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository.Models;
using WebShop.API.Extensions;
using WebShop.Services.Services;

namespace WebShop.Tests.Api.Tests
{
    public class OrderExtensionsTests
    {
        private Order _testOrder = new Order();

        [Fact]
        public async Task GetAllOrders_ReturnsOkResult()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();

            //Act
            var result = await OrderEndpointExtensions.GetAllOrders(fakeOrderService);

            //Assert
            var resultValue = Assert.IsType<Ok<IEnumerable<Order>>>(result);
            A.CallTo(() => fakeOrderService.GetAllOrders()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderById_ReturnsOkResult_WhenIdIsValid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int id = 1;

            //Act
            var result = await OrderEndpointExtensions.GetOrderById(fakeOrderService, id);

            //Assert
            var resultValue = Assert.IsType<Ok<Order>>(result);
            A.CallTo(() => fakeOrderService.GetOrderById(id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task GetOrderById_ReturnsNotFound_WhenIdIsInvalid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int id = 0;

            //Act
            var result = await OrderEndpointExtensions.GetOrderById(fakeOrderService, id);

            //Assert
            Assert.IsType<NotFound>(result);
            A.CallTo(() => fakeOrderService.GetOrderById(id)).MustNotHaveHappened();
        }
        [Fact]
        public async Task AddProductsToOrder_ReturnsOkResult_WhenOrderIdIsValid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int orderId = 1;
            var productIds = new List<int> { 1, 2, 3 };

            //Act
            var result = await OrderEndpointExtensions.AddProductsToOrder(fakeOrderService, orderId, productIds);

            //Assert
            var resultValue = Assert.IsType<Ok<string>>(result);
            A.CallTo(() => fakeOrderService.AddProductsToOrder(orderId, productIds)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task AddProductsToOrder_ReturnsNotFound_WhenOrderIdIsInvalid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int orderId = 0;
            var productIds = new List<int>();

            //Act
            var result = await OrderEndpointExtensions.AddProductsToOrder(fakeOrderService, orderId, productIds);

            //Assert
            Assert.IsType<NotFound>(result);
            A.CallTo(() => fakeOrderService.AddProductsToOrder(orderId, productIds)).MustNotHaveHappened();
        }
        [Fact]
        public async Task RemoveOrder_ReturnsOk_WhenIdIsValid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int id = 1;

            //Act
            var result = await OrderEndpointExtensions.RemoveOrder(fakeOrderService, id);

            //Assert
            var resultValue = Assert.IsType<Ok<string>>(result);
            A.CallTo(() => fakeOrderService.RemoveOrder(id)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RemoveOrder_ReturnsNotFound_WhenIdIsInvalid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int id = 0;

            //Act
            var result = await OrderEndpointExtensions.RemoveOrder(fakeOrderService, id);

            //Assert
            Assert.IsType<NotFound>(result);
            A.CallTo(() => fakeOrderService.RemoveOrder(id)).MustNotHaveHappened();
        }
        [Fact]
        public async Task UpdateOrder_ReturnsOk_WhenIdIsValid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            Order order = new Order();

            //Act
            var result = await OrderEndpointExtensions.UpdateOrder(fakeOrderService, order);

            //Assert
            var resultValue = Assert.IsType<Ok<string>>(result);
            A.CallTo(() => fakeOrderService.UpdateOrder(order)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task UpdateOrder_ReturnsProblem_WhenIdIsInvalid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            Order order = null!;

            //Act
            var result = await OrderEndpointExtensions.UpdateOrder(fakeOrderService, order);

            //Assert
            var resultValue = Assert.IsType<NotFound>(result);
            A.CallTo(() => fakeOrderService.UpdateOrder(order)).MustNotHaveHappened();
        }
        [Fact]
        public async Task UpdateOrderStatus_ReturnsOk_WhenIdIsValid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int id = 1;
            bool isShipped = true;

            //Act
            var result = await OrderEndpointExtensions.UpdateOrderStatus(fakeOrderService, id, isShipped);

            //Assert
            var resultValue = Assert.IsType<Ok<string>>(result);
            A.CallTo(() => fakeOrderService.UpdateOrderStatus(id, isShipped)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task UpdateOrderStatus_ReturnsNotFund_WhenIdIsInvalid()
        {
            //Arrange
            var fakeOrderService = A.Fake<IOrderService>();
            int id = 0;
            bool isShipped = true;

            //Act
            var result = await OrderEndpointExtensions.UpdateOrderStatus(fakeOrderService, id, isShipped);

            //Assert
            var resultValue = Assert.IsType<NotFound>(result);
            A.CallTo(() => fakeOrderService.UpdateOrderStatus(id, isShipped)).MustNotHaveHappened();
        }
    }
}
