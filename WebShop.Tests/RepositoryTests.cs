using FakeItEasy;
using Repository.Models;
using Repository.Repository;

namespace WebShop.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task Repository_AddProduct_ReturnShouldBeEqualToAdded()
        {
            // Arrange
            var testProduct = new Product { Id = 1, Description = "Test", Name = "Testprodukt", Price = 29.99, Stock = 50 };
            var repositoryFake = A.Fake<IProductRepository>();
            A.CallTo(() => repositoryFake.GetById(1)).Returns(testProduct);

            // Act
            await repositoryFake.Add(testProduct);
            var getProduct = await repositoryFake.GetById(1);

            // Assert
            A.CallTo(() => repositoryFake.Add(testProduct)).MustHaveHappenedOnceExactly();
            Assert.Equal(getProduct, testProduct);
        }
        [Fact]
        public async Task Repository_RemoveEntity_ShouldBeDeletedOnce()
        {
            // Arrange
            var testProduct = new Product { Id = 1, Description = "Test", Name = "Testprodukt", Price = 29.99, Stock = 50 };
            var repositoryFake = A.Fake<IProductRepository>();

            A.CallTo(() => repositoryFake.GetById(1)).Returns(Task.FromResult<Product>(testProduct));

            A.CallTo(() => repositoryFake.GetById(1)).Returns(Task.FromResult<Product>(null));



            // Act
            await repositoryFake.Remove(testProduct.Id);
            var getProduct = await repositoryFake.GetById(1);

            // Assert
            A.CallTo(() => repositoryFake.Remove(testProduct.Id)).MustHaveHappenedOnceExactly();
            Assert.Null(getProduct);
        }
    }
}
