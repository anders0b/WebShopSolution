using Microsoft.Data.SqlClient;
using Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Tests.Repository.Tests
{
    public class CustomerRepositoryTests
    {
        private Customer _testCustomer = new Customer { Email = "test@test.se", Phone = "0700000" };

        private SqlConnection _testConnection = new SqlConnection("Data Source = localhost; Initial Catalog = TEST_WebShopSQL; Integrated Security = True; Connect Timeout = 30; Encrypt=True;Trust Server Certificate=True;Application Intent = ReadWrite; Multi Subnet Failover=False");
        private readonly SqlTransaction _transaction;

        [Fact]
        public async Task CustomerRepository_GetCustomerByEmail_ShouldReturnCustomer()
        {
            //Arrange
            var repositoryFake = new CustomerRepository(_testConnection, _transaction);

            var email = "test@test.se";

            var customerId = await repositoryFake.Add(_testCustomer);

            //Act
            var customer = await repositoryFake.GetCustomerByEmail(email);

            //Assert
            Assert.NotNull(customer);
            Assert.Equal(email, customer.Email);
        }
        [Fact]
        public async Task CustomerRepository_GetCustomerByPhone_ShouldReturnCustomer()
        {
            //Arrange
            var repositoryFake = new CustomerRepository(_testConnection, _transaction);

            var phone = "0700000";

            var customerId = await repositoryFake.Add(_testCustomer);

            //Act
            var customer = await repositoryFake.GetCustomerByPhone(phone);

            //Assert
            Assert.NotNull(customer);
            Assert.Equal(phone, customer.Phone);
        }
        [Fact]
        public async Task CustomerRepository_UpdateCustomerEmail_ShouldReturnNewEmail()
        {
            //Arrange
            var repositoryFake = new CustomerRepository(_testConnection, _transaction);

            var newEmail = "ny@test.se";

            var customerId = await repositoryFake.Add(_testCustomer);

            //Act
            await repositoryFake.UpdateCustomerEmail(customerId, newEmail);

            //Assert
            var customer = await repositoryFake.GetById(customerId);
            Assert.Equal(newEmail, customer.Email);
        }
        [Fact]
        public async Task CustomerRepository_UpdateCustomerPhone_ShouldReturnNewPhone()
        {
            //Arrange
            var repositoryFake = new CustomerRepository(_testConnection, _transaction);

            var newPhone = "071111111";

            var customerId = await repositoryFake.Add(_testCustomer);

            //Act
            await repositoryFake.UpdateCustomerPhone(customerId, newPhone);

            //Assert
            var customer = await repositoryFake.GetById(customerId);
            Assert.Equal(newPhone, customer.Phone);
        }
        [Fact]
        public async Task CustomerRepository_GetCustomerFromOrder_ShouldReturnCorrectCustomer()
        {
            //Arrange
            var customerRepositoryFake = new CustomerRepository(_testConnection, _transaction);
            var orderRepositoryFake = new OrderRepository(_testConnection, _transaction);

            var orderId = await orderRepositoryFake.Add(new Order { OrderDate = DateTime.Now, IsShipped = false });
            var customerId = await customerRepositoryFake.Add(_testCustomer);

            //Act
            await customerRepositoryFake.GetCustomerFromOrder(orderId);

            //Assert
            var customer = await customerRepositoryFake.GetById(customerId);
            var order = await orderRepositoryFake.GetById(orderId);
            order.Customer = customer;
            Assert.Equal(customer.Id, order.Customer.Id);
        }
    }
}
