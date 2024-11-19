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
        private readonly IDbTransaction _transaction;
        public ICustomerRepository Customers{ get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        private bool _disposed;

        //private readonly ProductSubject _productSubject;

        //Konstruktor används för tillfället av Observer pattern
        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection ?? throw new Exception("Connection is null");
            _connection.Open();

            _transaction = _connection.BeginTransaction();

            Products = new ProductRepository(_connection, _transaction);
            Orders = new OrderRepository(_connection, _transaction);
            Customers = new CustomerRepository(_connection, _transaction);

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
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {

                _transaction.Rollback();
                throw;
            }
        }
    }
}

