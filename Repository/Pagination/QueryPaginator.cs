using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Pagination
{
    public static class QueryPaginator
    {
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query, int pageNumber, int pageSize) where T : class
        {
            var result = new PagedResult<T>();
            result.CurrentPage = pageNumber;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skipRows = (pageNumber - 1) * pageSize;
            result.Results = await query.Skip(skipRows).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
