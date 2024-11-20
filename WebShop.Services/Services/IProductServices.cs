using Repository.Models;
using WebShop.Repository.Repository;

namespace WebShop.Services.Services
{
    public interface IProductServices
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task RemoveProduct(Product product);
        Task UpdateProduct(Product product);
        Task UpdateStockQuantity(int id, int stock);
    }
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddProduct(Product product)
        {
            if(product.Price < 0)
            {
                throw new Exception("Price cannot be negative");
            }
            await _unitOfWork.Products.Add(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _unitOfWork.Products.GetAll();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _unitOfWork.Products.GetById(id);
        }

        public async Task RemoveProduct(Product entity)
        {
            await _unitOfWork.Products.Remove(entity);
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
    }
}
