using Microsoft.Extensions.Logging;
using Repository.Models;
using WebShop.Repository.Notifications.Factory;
using WebShop.Repository.Repository;

namespace WebShop.Services.Services
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task RemoveProduct(int id);
        Task UpdateProduct(Product product);
        Task UpdateStockQuantity(int id, int stock);
        Task<IEnumerable<Product>> GetAllProductsFromOrder(int orderId);
    }
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerFactory _loggerFactory;
        public ProductServices(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            _unitOfWork = unitOfWork;
            _loggerFactory = loggerFactory;
        }
        public async Task AddProduct(Product product) //implementation av factory- och observer pattern
        {
            await _unitOfWork.AttachObserver(new EmailNotificationFactory());
            await _unitOfWork.AttachObserver(new LoggerFactory(_loggerFactory));

            if (product.Price < 0)
            {
                throw new Exception("Price cannot be negative");
            }
            await _unitOfWork.Products.Add(product);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.NotifyProductAdded(product);
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _unitOfWork.Products.GetAll();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _unitOfWork.Products.GetById(id);
        }

        public async Task RemoveProduct(int id)
        {
            await _unitOfWork.Products.Remove(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product entity)
        {
            await _unitOfWork.Products.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateStockQuantity(int id, int stock)
        {
            await _unitOfWork.Products.UpdateStockQuantity(id, stock);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<Product>> GetAllProductsFromOrder(int orderId)
        {
            return await _unitOfWork.Products.GetAllProductsFromOrder(orderId);
        }
    }
}
