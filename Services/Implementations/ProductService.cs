using Domain.Abstractions;
using Domain.Entities;
using System;
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

        public async Task<PagedResult<Product>> GetProducts(FilterParams filters)
        {
            return await _productRepository.GetProductsAsync(filters);
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var existingProduct = await _productRepository
                .GetProductByCodeAsync(product.Code);

            if (existingProduct != null)
            {
                throw new ApplicationException("Another product exists with the same code.");
            }

            return await _productRepository.CreateProductAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var existingProduct = await _productRepository
                .GetProductByCodeAsync(product.Code);

            if (existingProduct != null && product.Id != existingProduct.Id)
            {
                throw new ApplicationException("Another product exists with the same code.");
            }

            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            return await _productRepository.DeleteProductAsync(product);
        }
    }
}
