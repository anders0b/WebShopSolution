using FakeItEasy;
using Microsoft.Extensions.Logging;
using Repository.Models;
using WebShop.Repository.Notifications;

namespace WebShop.Tests.Observer.Tests
{
    public class ObserverPatternTests
    {
        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate_WithCorrectProduct()
        {
            //Arrange
            var product = new Product { Id = 1, Name = "Test" };

            var fakeObserver = A.Fake<INotificationObserver>();
            var fakeFactory = A.Fake<INotificationObserverFactory>();
            A.CallTo(() => fakeFactory.CreateNotificationObserver()).Returns(fakeObserver);

            var productSubject = new ProductSubject();
            productSubject.Attach(fakeFactory);

            //Act
            productSubject.Notify(product);

            //Assert
            A.CallTo(() => fakeObserver.Update(product)).MustHaveHappenedOnceExactly();
        }
    }
}
