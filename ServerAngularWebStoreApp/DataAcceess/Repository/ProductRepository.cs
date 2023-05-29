using Common.Models;
using DataAcceess.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAcceess.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DataAccessContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsByIds(List<int> ids)
        {
            return table.Where(p => ids.Contains(p.Id));
        }
    }
}