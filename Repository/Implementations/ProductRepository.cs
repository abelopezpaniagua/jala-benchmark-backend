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
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            
            return await _context.SaveChangesAsync() > 0 ? product : null;
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
