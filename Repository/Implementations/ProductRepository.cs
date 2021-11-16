using Domain. Entities;
using Domain.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repository.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqliteDbContext _context;

        public ProductRepository(SqliteDbContext context)
        {
            //Initialize db context
            _context = context;
        }

        /// <summary>
        /// Register a new product asynchronously
        /// </summary>
        /// <param name="product">Product entity</param>
        /// <returns>If product is created successfully returns that product otherwise return null.</returns>
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            
            return await _context.SaveChangesAsync() > 0 ? product : null;
        }

        /// <summary>
        /// Delete an existing product asynchronously
        /// </summary>
        /// <param name="product">Existing product entity</param>
        /// <returns>Returns true if the product is successfully deleted otherwise returns false.</returns>
        public async Task<bool> DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Retrieve an existing product asynchronously using the product ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Returns the existing product otherwise returns null.</returns>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Retrieve all existing products asynchronously
        /// </summary>
        /// <returns>Returns the collection of existing products.</returns>
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Update an existing product asynchronously
        /// </summary>
        /// <param name="product">Existing product entity</param>
        /// <returns>Returns true if the product is successfully updated otherwise returns false.</returns>

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
