using FakeItEasy;
using Repository.Models;
using WebShop.Repository.Notifications;
using WebShop.Repository.Notifications.Factory;

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

            var productSubject = new EntitySubject<Product>();
            productSubject.Attach(fakeFactory);

            //Act
            productSubject.Notify(product);

            //Assert
            A.CallTo(() => fakeObserver.Update(product)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public void NotifyOrderAdded_CallsObserverUpdate_WithCorrectOrder()
        {
            //Arrange
            var order = new Order { Id = 1 };

            var fakeObserver = A.Fake<INotificationObserver>();
            var fakeFactory = A.Fake<INotificationObserverFactory>();
            A.CallTo(() => fakeFactory.CreateNotificationObserver()).Returns(fakeObserver);

            var orderSubject = new EntitySubject<Order>();
            orderSubject.Attach(fakeFactory);

            //Act
            orderSubject.Notify(order);

            //Assert
            A.CallTo(() => fakeObserver.Update(order)).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public void NotifyCustomerAdded_CallsObserverUpdate_WithCorrectCustomer()
        {
            //Arrange
            var customer = new Customer { Id = 1 };

            var fakeObserver = A.Fake<INotificationObserver>();
            var fakeFactory = A.Fake<INotificationObserverFactory>();
            A.CallTo(() => fakeFactory.CreateNotificationObserver()).Returns(fakeObserver);

            var customerSubject = new EntitySubject<Customer>();
            customerSubject.Attach(fakeFactory);

            //Act
            customerSubject.Notify(customer);

            //Assert
            A.CallTo(() => fakeObserver.Update(customer)).MustHaveHappenedOnceExactly();
        }
    }
}
