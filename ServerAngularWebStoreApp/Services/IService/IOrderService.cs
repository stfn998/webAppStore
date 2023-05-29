using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IOrderService
    {
        Task<bool> AddOrder(OrderDTO dto);

        Task<IEnumerable<OrderDTO>> GetAll();

        Task<OrderDTO> GetOne(int id);

        Task<bool> OrderConfirmation(int idOrder);

        Task<IEnumerable<ProductDTO>> GetProductsFromOrder(int idOrder);

        Task<IEnumerable<OrderDTO>> GetOrdersByIdCustomer(int idPerson);

        Task<IEnumerable<OrderDTO>> GetOrdersByIdDeliverer(int idPerson);

        Task<OrderDTO> LastOrder(int IdCustomer);
    }
}