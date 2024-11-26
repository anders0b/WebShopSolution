using FakeItEasy;
using Repository.Models;
using WebShop.Repository.Notifications;
using WebShop.Repository.Repository;

namespace WebShop.Tests.Observer.Tests
{
    public class ObserverPatternTests
    {
        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test" };

            var fakeObserver = A.Fake<INotificationObserver>();
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();

            var productSubject = new ProductSubject();
            productSubject.Attach(fakeObserver);

            A.CallTo(() => fakeUnitOfWork.NotifyProductAdded(product)).Invokes(async () => await productSubject.Notify(product));

            // Act
            fakeUnitOfWork.NotifyProductAdded(product);

            // Assert

            A.CallTo(() => fakeObserver.Update(product)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate_WithCorrectProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test" };

            var fakeObserver = A.Fake<INotificationObserver>();
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();

            var productSubject = new ProductSubject();
            productSubject.Attach(fakeObserver);

            A.CallTo(() => fakeUnitOfWork.NotifyProductAdded(product)).Invokes(async () => await productSubject.Notify(product));

            // Act
            fakeUnitOfWork.NotifyProductAdded(product);

            // Assert

            A.CallTo(() => fakeObserver.Update(product)).MustHaveHappenedOnceExactly();
        }
    }
}
