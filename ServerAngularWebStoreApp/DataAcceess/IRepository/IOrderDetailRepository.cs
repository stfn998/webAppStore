using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAcceess.IRepository
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetByIdOrder(int idOrder);
    }
}