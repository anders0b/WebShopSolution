using FakeItEasy;
using Repository.Models;
using Repository.Repository;

namespace WebShop.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void Repository_AddProduct_ReturnShouldBeEqualToAdded()
        {
            // Arrange
            var testProduct = new Product { Id = 1, Description = "Test", Name = "Testprodukt", Price = 29.99, Stock = 50 };
            var repositoryFake = A.Fake<IProductRepository>();
            A.CallTo(() => repositoryFake.GetById(1)).Returns(testProduct);

            // Act
            repositoryFake.Add(testProduct);
            var getProduct = repositoryFake.GetById(1);

            // Assert
            A.CallTo(() => repositoryFake.Add(testProduct)).MustHaveHappenedOnceExactly();
            Assert.Equal(getProduct, testProduct);
        }
    }
}
