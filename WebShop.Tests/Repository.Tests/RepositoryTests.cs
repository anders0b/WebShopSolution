using FakeItEasy;
using Repository.Repository;


namespace WebShop.Tests.Repository.Tests;


public class RepositoryTests
{
    private object _testObject = new Object();

    [Fact]
    public async Task Repository_AddProduct_ReturnShouldBeEqualToAdded()
    {
        // Arrange
        var repositoryFake = A.Fake<IRepository<object>>();
        A.CallTo(() => repositoryFake.Add(_testObject)).Returns(1);

        // Act
        var id = await repositoryFake.Add(_testObject);

        // Assert
        A.CallTo(() => repositoryFake.Add(_testObject)).MustHaveHappenedOnceExactly();
        Assert.Equal(1, id);

    }
    [Fact]
    public async Task Repository_RemoveEntity_ShouldBeDeletedOnce()
    {
        // Arrange
        var repositoryFake = A.Fake<IRepository<object>>();

        A.CallTo(() => repositoryFake.Remove(1));

        // Act
        await repositoryFake.Remove(1);

        // Assert
        A.CallTo(() => repositoryFake.Remove(1)).MustHaveHappenedOnceExactly();
    }
    [Fact]
    public async Task Repository_GetAllEntities_ShouldReturnCurrentListOrNew()
    {
        //Arrange
        var repositoryFake = A.Fake<IRepository<object>>();
        var initialList = new List<object> { _testObject };
        A.CallTo(() => repositoryFake.GetAll()).Returns(initialList);

        //Act
        var newlist = await repositoryFake.GetAll();

        //Assert
        Assert.Equal(initialList.Count, newlist.Count());
    }
    [Fact]
    public async Task Repository_UpdateEntity_ShouldReturnUpdatedEntity()
    {
        //Arrange
        var repositoryFake = A.Fake<IRepository<object>>();
        var updatedObject = new Object();
        A.CallTo(() => repositoryFake.Update(updatedObject));

        //Act
        await repositoryFake.Update(updatedObject);

        //Assert
        A.CallTo(() => repositoryFake.Update(updatedObject)).MustHaveHappenedOnceExactly();
    }
}
