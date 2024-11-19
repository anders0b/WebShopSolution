using Repository.Repository;
using System.Data;

namespace WebShop.Repository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        Task SaveChangesAsync();
        //void NotifyProductAdded(Product product); // Notifierar observatörer om ny produkt
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        public ICustomerRepository Customers{ get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }

        //private readonly ProductSubject _productSubject;

        //Konstruktor används för tillfället av Observer pattern
        public UnitOfWork(IDbConnection connection, ICustomerRepository customerRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _connection = connection;
            Customers = customerRepository;
            Products = productRepository;
            Orders = orderRepository;

            //Products = null;

            // Om inget ProductSubject injiceras, skapa ett nytt
            //_productSubject = productSubject ?? new ProductSubject();

            // Registrera standardobservatörer
            //_productSubject.Attach(new EmailNotification());
        }
        //public void NotifyProductAdded(Product product)
        //{
        //    _productSubject.Notify(product);
        //}
        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            if(_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
    }
}

