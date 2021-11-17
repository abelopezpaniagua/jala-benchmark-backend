using System.Collections.Generic;

namespace Domain.Entities
{
    public  class PagedResult<T> : PagedResultBase where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
