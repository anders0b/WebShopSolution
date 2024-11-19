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
            var products = await _unitOfWork.Products.GetAll();
            return products;
        }

        public Task<Product> GetProductById(int id)
        {
            return _unitOfWork.Products.GetById(id);
        }

        public Task RemoveProduct(Product entity)
        {
            return _unitOfWork.Products.Remove(entity);
        }

        public Task UpdateProduct(Product entity)
        {
            return _unitOfWork.Products.Update(entity);
        }
    }
}
