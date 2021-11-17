using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IProductService
    {
        /// <summary>
        /// Retrieve all existing products that match with the filters
        /// </summary>
        /// <param name="filters">Filters object to define search filter and pagination filter</param>
        /// <returns>Returns the collection of existing products that match with filters.</returns>
        Task<PagedResult<Product>> GetProducts(FilterParams filters);

        /// <summary>
        /// Retrieve an existing product using the product ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Returns the existing product otherwise returns null.</returns>
        Task<Product> GetProductById(int id);

        /// <summary>
        /// Register a new product
        /// </summary>
        /// <param name="product">Product entity</param>
        /// <returns>If product is created successfully returns that product otherwise return null.</returns>
        Task<Product> CreateProduct(Product product);

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="product">Existing product entity</param>
        /// <returns>Returns true if the product is successfully updated otherwise returns false.</returns>
        Task<bool> UpdateProduct(Product product);

        /// <summary>
        /// Delete an existing product
        /// </summary>
        /// <param name="product">Existing product entity</param>
        /// <returns>Returns true if the product is successfully deleted otherwise returns false.</returns>
        Task<bool> DeleteProduct(Product product);
    }
}
