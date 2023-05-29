using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAcceess.IRepository
{
    public interface IOrderRepository
    {
        Task<Order> GetLast(int IdCustomer);

        Task<IEnumerable<Order>> GetOrdersByIdCustomer(int idPerson);

        Task<IEnumerable<Order>> GetOrdersByIdSeller(int idPerson);

        Task<Order> GetOrderById(int id);
    }
}