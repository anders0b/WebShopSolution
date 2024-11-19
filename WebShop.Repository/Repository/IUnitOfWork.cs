using Repository.Repository;
using System.Data;

namespace WebShop.Repository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<int> SaveChangesAsync();
        //void NotifyProductAdded(Product product); // Notifierar observatörer om ny produkt
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        private readonly Dictionary<Type, object> _repositories;
        public ICustomerRepository CustomerRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IOrderRepository OrderRepository { get; }

        //private readonly ProductSubject _productSubject;

        //Konstruktor används för tillfället av Observer pattern
        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection ?? throw new Exception("Connection is null");
            _connection.Open();

            _transaction = _connection.BeginTransaction();

            _repositories = new Dictionary<Type, object>();

            ProductRepository = new ProductRepository(_connection, _transaction);
            OrderRepository = new OrderRepository(_connection, _transaction);
            CustomerRepository = new CustomerRepository(_connection, _transaction);

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
            _transaction.Dispose();
            _connection.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                _transaction.Commit();
                return 1;
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
        }


    }
}

