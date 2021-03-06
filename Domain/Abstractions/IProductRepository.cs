using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieve all existing products asynchronously
        /// </summary>
        /// <param name="filters">Filters object to define search filter and pagination filter</param>
        /// <returns>Returns the collection of existing products.</returns>
        Task<PagedResult<Product>> GetProductsAsync(FilterParams filters);

        /// <summary>
        /// Retrieve an existing product asynchronously using the product ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Returns the existing product otherwise returns null.</returns>
        Task<Product> GetProductByIdAsync(int id);

        /// <summary>
        /// Retrieve an existing product asynchronously using the product Code
        /// </summary>
        /// <param name="code">Product Code</param>
        /// <returns>Returns the existing product otherwise returns null.</returns>
        Task<Product> GetProductByCodeAsync(string code);
        
        /// <summary>
        /// Register a new product asynchronously
        /// </summary>
        /// <param name="product">Product entity</param>
        /// <returns>If product is created successfully returns that product otherwise return null.</returns>
        Task<Product> CreateProductAsync(Product product);

        /// <summary>
        /// Update an existing product asynchronously
        /// </summary>
        /// <param name="product">Existing product entity</param>
        /// <returns>Returns true if the product is successfully updated otherwise returns false.</returns>
        Task<bool> UpdateProductAsync(Product product);
        
        /// <summary>
        /// Delete an existing product asynchronously
        /// </summary>
        /// <param name="product">Existing product entity</param>
        /// <returns>Returns true if the product is successfully deleted otherwise returns false.</returns>
        Task<bool> DeleteProductAsync(Product product);
    }
}
