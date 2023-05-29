﻿using Common.Models;
using DataAcceess.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAcceess.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(DataAccessContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetByIdOrder(int idOrder)
        {
            return table.Where(op => op.OrderId == idOrder);
        }
    }
}