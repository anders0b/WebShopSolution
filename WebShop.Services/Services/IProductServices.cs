using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            await _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return _unitOfWork.ProductRepository.GetAll();
        }

        public Task<Product> GetProductById(int id)
        {
            return _unitOfWork.ProductRepository.GetById(id);
        }

        public Task RemoveProduct(Product entity)
        {
            return _unitOfWork.ProductRepository.Remove(entity);
        }

        public Task UpdateProduct(Product entity)
        {
            return _unitOfWork.ProductRepository.Update(entity);
        }
    }
}
