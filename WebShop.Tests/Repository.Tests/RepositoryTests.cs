using Dapper;
using FakeItEasy;
using Microsoft.Data.SqlClient;
using Repository.Models;
using Repository.Repository;
using WebShop.Repository.Repository;


namespace WebShop.Tests.Repository.Tests
{
    public class RepositoryTests
    {
        private Product _testProduct = new Product { Description = "Test", Name = "Testprodukt", Price = 29.99, Stock = 50 };
        private SqlConnection _testConnection = new SqlConnection("Data Source = localhost; Initial Catalog = TEST_WebShopSQL; Integrated Security = True; Connect Timeout = 30; Encrypt=True;Trust Server Certificate=True;Application Intent = ReadWrite; Multi Subnet Failover=False");
        private readonly SqlTransaction _transaction;

        [Fact]
        public async Task Repository_AddProduct_ReturnShouldBeEqualToAdded()
        {
            // Arrange
            var repositoryFake = new ProductRepository(_testConnection, _transaction);

            // Act
            var id = await repositoryFake.Add(_testProduct);
            _testProduct.Id = id;
            var getProduct = await repositoryFake.GetById(id);

            // Assert
            Assert.Equal(_testProduct.Id, getProduct.Id);
            Assert.Equal(_testProduct.Name, getProduct.Name);
            Assert.Equal(_testProduct.Description, getProduct.Description);
            Assert.Equal(_testProduct.Price, getProduct.Price);
            Assert.Equal(_testProduct.Stock, getProduct.Stock);
        }
        [Fact]
        public async Task Repository_RemoveEntity_ShouldBeDeletedOnce()
        {
            // Arrange
            var repositoryFake = new ProductRepository(_testConnection, _transaction);

            var id = await repositoryFake.Add(_testProduct);
            _testProduct.Id = id;

            // Act
            await repositoryFake.Remove(id);
            var getProduct = await repositoryFake.GetById(id);

            // Assert
            Assert.Null(getProduct);
        }
        [Fact]
        public async Task Repository_GetAllEntities_ShouldReturnCurrentListOrNew()
        {
            //Arrange
            var repositoryFake = new ProductRepository(_testConnection, _transaction);
            var initialList = await repositoryFake.GetAll();
            await repositoryFake.Add(_testProduct);

            //Act
            var newlist = await repositoryFake.GetAll();

            //Assert
            var initialListCount = initialList.Count() + 1;
            var newlistCount = newlist.Count();

            Assert.Equal(initialListCount, newlistCount);
        }
        [Fact]
        public async Task Repository_UpdateEntity_ShouldReturnUpdatedEntity()
        {
            //Arrange
            var testRepository = new ProductRepository(_testConnection, _transaction);

            var id = await testRepository.Add(_testProduct);
            var updatedTestProduct = new Product { Id = id, Description = "NyTest", Name = "NyTestProdukt", Price = 59.99, Stock = 49 };


            //Act
            await testRepository.Update(updatedTestProduct);
            var getProduct = await testRepository.GetById(id);

            //Assert
            Assert.NotNull(getProduct);
            Assert.Equal("NyTestProdukt", getProduct.Name);
            Assert.Equal("NyTest", getProduct.Description);
            Assert.Equal(59.99, getProduct.Price);
            Assert.Equal(49, getProduct.Stock);
        }
    }

}
