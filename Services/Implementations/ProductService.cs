using Domain.Abstractions;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productRepository.GetProductsAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            return await _productRepository.CreateProductAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            return await _productRepository.DeleteProductAsync(product);
        }
    }
}
