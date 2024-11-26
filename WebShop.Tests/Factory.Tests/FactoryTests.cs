
using FakeItEasy;
using WebShop.Repository.Notifications;

namespace WebShop.Tests.Factory.Tests;

public class FactoryTests
{
    [Fact]
    public void Factory_ReturnsValidObserverInstance()
    {
        //Arrange
        var fakeFactory = A.Fake<INotificationObserverFactory>();
        var fakeObserver = A.Fake<INotificationObserver>();
        A.CallTo(() => fakeFactory.CreateNotificationObserver()).Returns(fakeObserver);

        //Act
        var observer = fakeFactory.CreateNotificationObserver();

        //Assert
        Assert.NotNull(observer);
        Assert.IsAssignableFrom<INotificationObserver>(observer);
    }
    [Fact]
    public void Factory_ReturnsDifferentObserverInstances()
    {
        //Arrange
        var fakeFactory = A.Fake<INotificationObserverFactory>();
        var fakeObserver1 = A.Fake<INotificationObserver>();
        var fakeObserver2 = A.Fake<INotificationObserver>();
        A.CallTo(() => fakeFactory.CreateNotificationObserver()).ReturnsNextFromSequence(fakeObserver1, fakeObserver2);

        //Act
        var observer1 = fakeFactory.CreateNotificationObserver();
        var observer2 = fakeFactory.CreateNotificationObserver();

        //Assert
        Assert.NotSame(observer1, observer2);
    }
}
