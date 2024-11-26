using FakeItEasy;
using Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Tests.Repository.Tests
{
    public class CustomerRepositoryTests
    {
        private Customer _testCustomer = new Customer { Id = 1, Email = "test@test.se", Phone = "0700000" };

        [Fact]
        public async Task CustomerRepository_GetCustomerByEmail_ShouldReturnCustomer()
        {
            //Arrange
            var repositoryFake = A.Fake<ICustomerRepository>();
            var unitOfWorkFake = A.Fake<IUnitOfWork>();
            var email = "test@test.se";

            A.CallTo(() => unitOfWorkFake.Customers.GetCustomerByEmail(email)).Returns(_testCustomer);

            //Act
            var customer = await unitOfWorkFake.Customers.GetCustomerByEmail(email);

            //Assert
            A.CallTo(() => unitOfWorkFake.Customers.GetCustomerByEmail(email)).MustHaveHappenedOnceExactly();
            Assert.NotNull(customer);
            Assert.Equal(email, customer.Email);
        }
        [Fact]
        public async Task CustomerRepository_GetCustomerByPhone_ShouldReturnCustomer()
        {
            //Arrange
            var repositoryFake = A.Fake<ICustomerRepository>();
            var unitOfWorkFake = A.Fake<IUnitOfWork>();

            var phone = "0700000";

            A.CallTo(() => repositoryFake.GetCustomerByPhone(phone)).Returns(_testCustomer);
            A.CallTo(() => unitOfWorkFake.Customers).Returns(repositoryFake);

            //Act
            var customer = await unitOfWorkFake.Customers.GetCustomerByPhone(phone);

            //Assert
            A.CallTo(() => unitOfWorkFake.Customers.GetCustomerByPhone(phone)).MustHaveHappenedOnceExactly();
            Assert.NotNull(customer);
            Assert.Equal(phone, customer.Phone);
        }
        [Fact]
        public async Task CustomerRepository_UpdateCustomerEmail_ShouldReturnNewEmail()
        {
            //Arrange
            var repositoryFake = A.Fake<ICustomerRepository>();
            var unitOfWorkFake = A.Fake<IUnitOfWork>();

            var newEmail = "ny@test.se";

            A.CallTo(() => repositoryFake.UpdateCustomerEmail(_testCustomer.Id, newEmail)).Invokes(() => _testCustomer.Email = newEmail);
            A.CallTo(() => unitOfWorkFake.Customers).Returns(repositoryFake);

            //Act
            await unitOfWorkFake.Customers.UpdateCustomerEmail(_testCustomer.Id, newEmail);

            //Assert
            A.CallTo(() => unitOfWorkFake.Customers.UpdateCustomerEmail(1, newEmail)).MustHaveHappenedOnceExactly();
            Assert.Equal(newEmail, _testCustomer.Email);
        }
        [Fact]
        public async Task CustomerRepository_UpdateCustomerPhone_ShouldReturnNewPhone()
        {
            //Arrange
            var repositoryFake = A.Fake<ICustomerRepository>();
            var unitOfWorkFake = A.Fake<IUnitOfWork>();

            var newPhone = "071111111";

            A.CallTo(() => repositoryFake.UpdateCustomerPhone(_testCustomer.Id, newPhone)).Invokes(() => _testCustomer.Phone = newPhone);
            A.CallTo(() => unitOfWorkFake.Customers).Returns(repositoryFake);

            //Act
            await unitOfWorkFake.Customers.UpdateCustomerPhone(_testCustomer.Id, newPhone);

            //Assert
            A.CallTo(() => unitOfWorkFake.Customers.UpdateCustomerPhone(_testCustomer.Id, newPhone)).MustHaveHappenedOnceExactly();
            Assert.Equal(newPhone, _testCustomer.Phone);
        }
        [Fact]
        public async Task CustomerRepository_GetCustomerFromOrder_ShouldReturnCorrectCustomer()
        {
            //Arrange
            var customerRepositoryFake = A.Fake<ICustomerRepository>();
            var unitOfWorkFake = A.Fake<IUnitOfWork>();
            var orderId = 1;
            var newOrder = new Order { Id = orderId, Customer = _testCustomer };

            A.CallTo(() => customerRepositoryFake.GetCustomerFromOrder(orderId)).Returns(_testCustomer);
            A.CallTo(() => unitOfWorkFake.Customers).Returns(customerRepositoryFake);

            //Act
            await unitOfWorkFake.Customers.GetCustomerFromOrder(orderId);

            //Assert
            A.CallTo(() => customerRepositoryFake.GetCustomerFromOrder(orderId)).MustHaveHappenedOnceExactly();
            Assert.NotNull(_testCustomer);
            Assert.Equal(_testCustomer.Id, newOrder.Customer.Id);
        }
    }
}
