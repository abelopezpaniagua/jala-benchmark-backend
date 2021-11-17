using System.Linq;
using Domain. Entities;
using Domain.Abstractions;
using Repository.Pagination;
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

        public async Task<PagedResult<Product>> GetProductsAsync(FilterParams filters)
        {
            string searchFilter = filters.SearchFilter?.Trim().ToLower();

            var query = _context.Products
                .OrderBy(p => p.Code)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = query
                    .Where(p => p.Code.ToLower().Contains(searchFilter)
                        || p.Name.ToLower().Contains(searchFilter));
            }

            return await query.GetPaged(filters.PageNumber, filters.PageSize);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            
            return await _context.SaveChangesAsync() > 0 ? product : null;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
