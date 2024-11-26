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

            var productSubject = new ProductSubject();
            productSubject.Attach(new ProductLoggerFactory(A.Fake<ILoggerFactory>()));

            A.CallTo(() => fakeObserver.Update(product)).DoesNothing();

            //Act
            productSubject.Notify(product);

            //Assert

            A.CallTo(() => fakeObserver.Update(product)).MustHaveHappenedOnceExactly();
        }
    }
}
