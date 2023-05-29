using Common.Models;
using DataAcceess.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcceess.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(DataAccessContext context) : base(context)
        {
        }

        public async Task<Order> GetLast(int IdCustomer)
        {
            var orders = table.Where(o => o.CustomerId == IdCustomer);
            Order order = await orders.OrderByDescending(o => o.Id).FirstOrDefaultAsync();
            return order;
        }

        public async Task<Order> GetOrderById(int id)
        {
            var orders = table.Where(o => o.Id == id);
            Order order = await orders.OrderByDescending(o => o.Id).FirstOrDefaultAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByIdCustomer(int idPerson)
        {
            var orders = table.Where(o => o.CustomerId == idPerson);
            return orders;
        }

        public async Task<IEnumerable<Order>> GetOrdersByIdSeller(int idPerson)
        {
            var orders = await table
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Product)
            .Where(o => o.OrderDetails.Any(od => od.Product.SellerId == idPerson))
            .ToListAsync();

            return orders;
        }
    }
}