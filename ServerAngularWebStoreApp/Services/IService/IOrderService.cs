using Common.DTOs;
using Common.Models;
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

        Task<OrderDetailDTO> CreateOrder(OrderDetailDTO dto);

        Task<OrderDetailDTO> AddProduct(OrderDetailDTO dto);

        Task<bool> RemoveProduct(OrderDetailDTO dto);

        Task<IEnumerable<OrderDTO>> GetAll();

        Task<OrderDTO> GetOrder(int idOrder);

        Task<IEnumerable<ProductDTO>> GetProductsFromOrder(int idOrder);

        Task<IEnumerable<OrderDTO>> GetOrdersByIdCustomer(int idPerson);

        Task<IEnumerable<OrderDTO>> GetOrdersByIdDeliverer(int idPerson);

        Task<OrderDTO> LastOrder(int IdCustomer);
    }
}