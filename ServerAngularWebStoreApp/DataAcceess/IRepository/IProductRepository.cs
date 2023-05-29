using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcceess.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByIds(List<int> ids);
    }
}