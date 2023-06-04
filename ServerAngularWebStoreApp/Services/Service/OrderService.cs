using AutoMapper;
using Common.DTOs;
using Common.Models;
using DataAcceess.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Product> _genericRepositoryProduct;
        private readonly IGenericRepository<Order> _genericRepositoryOrder;
        private readonly IGenericRepository<OrderDetail> _genericRepositoryOrderDetail;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Product> genericRepositoryProduct, IGenericRepository<Order> genericRepositoryOrder, IGenericRepository<OrderDetail> genericRepositoryOrderDetail, IOrderRepository orderRepository, IProductRepository productRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _genericRepositoryProduct = genericRepositoryProduct;
            _genericRepositoryOrder = genericRepositoryOrder;
            _genericRepositoryOrderDetail = genericRepositoryOrderDetail;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddOrder(OrderDTO dto)
        {
            Order order = _mapper.Map<Order>(dto);

            await _genericRepositoryOrder.Insert(order);
            await _genericRepositoryOrder.Save();

            /*Order newOrder = await _orderRepository.GetLast(dto.IdCustomer);
            if (newOrder == null)
            {
                throw new KeyNotFoundException("Order does not exists.");
            }

            foreach (ProductDTO product in dto.Products)
            {
                OrderProduct op = new OrderProduct
                {
                    IdOrder = newOrder.Id,
                    IdProduct = product.Id
                };
                await _genericRepositoryOrderProduct.Insert(op);
            }

            await _genericRepositoryOrderProduct.Save();*/

            return true;
        }

        public async Task<IEnumerable<OrderDTO>> GetAll()
        {
            List<Product> products = new List<Product>();
            IEnumerable<Order> orders = await _genericRepositoryOrder.GetAll();
            if (orders == null)
            {
                throw new KeyNotFoundException("Currently there is no orders.");
            }

            IEnumerable<OrderDTO> ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
            foreach (OrderDTO oDto in ordersDto)
            {
                IEnumerable<OrderDetail> op = await _orderDetailRepository.GetByIdOrder(oDto.Id);
                foreach (OrderDetail opp in op)
                {
                    if (oDto.Id == opp.OrderId)
                    {
                        products.Add(await _genericRepositoryProduct.GetByObject(opp.OrderId));
                    }
                }
                //oDto.OrderDetails = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                products.Clear();
            }

            return ordersDto;
        }

        public async Task<OrderDTO> GetOne(int id)
        {
            Order order = await _genericRepositoryOrder.GetByObject(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exists.");
            }

            OrderDTO dto = _mapper.Map<OrderDTO>(order);

            return dto;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsFromOrder(int idOrder)
        {
            List<ProductDTO> dto = new List<ProductDTO>();
            Order order = await _genericRepositoryOrder.GetByObject(idOrder);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exist");
            }

            var orderProducts = await _orderDetailRepository.GetByIdOrder(idOrder);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not have products.");
            }

            foreach (var op in orderProducts)
            {
                Product product = await _genericRepositoryProduct.GetByObject(op.Product);
                ProductDTO pDto = _mapper.Map<ProductDTO>(product);
                dto.Add(pDto);
            }

            return dto;
        }

        public async Task<bool> OrderConfirmation(int idOrder)
        {
            Order order = await _genericRepositoryOrder.GetByObject(idOrder);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exist.");
            }

            // order.OrderStatus = Enums.OrderStatus.Done;

            await _genericRepositoryOrder.Update(order);
            await _genericRepositoryOrder.Save();

            return true;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByIdCustomer(int idPerson)
        {
            List<Product> products = new List<Product>();
            IEnumerable<Order> orders = await _orderRepository.GetOrdersByIdCustomer(idPerson);
            if (orders == null)
            {
                throw new KeyNotFoundException("User does not have orders.");
            }

            IEnumerable<OrderDTO> ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
            /*foreach (OrderDTO oDto in ordersDto)
            {
                IEnumerable<OrderProduct> op = await _orderProductRepository.GetByIdOrder(oDto.Id);
                foreach (OrderProduct opp in op)
                {
                    if (oDto.Id == opp.IdOrder)
                    {
                        products.Add(await _genericRepositoryProduct.GetByObject(opp.IdProduct));
                    }
                }
                oDto.Products = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                products.Clear();
            }*/

            return ordersDto;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByIdDeliverer(int idPerson)
        {
            List<Product> products = new List<Product>();
            IEnumerable<Order> orders = await _orderRepository.GetOrdersByIdSeller(idPerson);
            if (orders == null)
            {
                throw new KeyNotFoundException("User does not have orders.");
            }

            IEnumerable<OrderDTO> ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
            foreach (OrderDTO oDto in ordersDto)
            {
                IEnumerable<OrderDetail> op = await _orderDetailRepository.GetByIdOrder(oDto.Id);
                foreach (OrderDetail opp in op)
                {
                    if (oDto.Id == opp.OrderId)
                    {
                        products.Add(await _genericRepositoryProduct.GetByObject(opp.OrderId));
                    }
                }
                //oDto.Products = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);
                products.Clear();
            }

            return ordersDto;
        }

        public async Task<OrderDTO> LastOrder(int IdCustomer)
        {
            List<ProductDTO> porductDto = new List<ProductDTO>();
            Order order = await _orderRepository.GetLast(IdCustomer);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exists.");
            }

            OrderDTO dto = _mapper.Map<OrderDTO>(order);

            var orderProducts = await _orderDetailRepository.GetByIdOrder(order.Id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not have products.");
            }

            foreach (var op in orderProducts)
            {
                Product product = await _genericRepositoryProduct.GetByObject(op.ProductId);
                ProductDTO pDto = _mapper.Map<ProductDTO>(product);
                porductDto.Add(pDto);
            }
            //dto.Products = porductDto;

            return dto;
        }

        public async Task<int> CreateOrder(OrderDetailDTO dto)
        {
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(dto);

            Order order = new Order()
            {
                CustomerId = dto.CustomerId,
                DeliveryAddress = ""
            };

            if (orderDetail == null || order == null)
            {
                throw new KeyNotFoundException("Adding new order failed");
            }

            orderDetail.Quantity = 1;
            order.OrderDetails.Add(orderDetail);

            await _genericRepositoryOrder.Insert(order);
            await _genericRepositoryOrder.Save();

            return order.Id;
        }

        public async Task<int> AddProduct(OrderDetailDTO dto)
        {
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(dto);

            Order order = await _genericRepositoryOrder.GetByObject(orderDetail.OrderId);

            if (orderDetail == null || order == null)
            {
                throw new KeyNotFoundException("Adding new order failed");
            }

            orderDetail.Quantity = 1;
            order.OrderDetails.Add(orderDetail);

            await _genericRepositoryOrder.Update(order);
            await _genericRepositoryOrder.Save();

            return order.Id;
        }

        public async Task<bool> RemoveProduct(OrderDetailDTO dto)
        {
            IEnumerable<OrderDetail> orderDetailList = await _orderDetailRepository.GetByIdOrder(dto.OrderId);
            if (orderDetailList == null)
            {
                throw new KeyNotFoundException("Order does not exist");
            }

            OrderDetail orderDetail = orderDetailList.FirstOrDefault(o => o.ProductId.Equals(dto.ProductId));

            if (orderDetail == null)
            {
                throw new KeyNotFoundException("Product does not exist.");
            }
            else
            {
                await _genericRepositoryOrderDetail.Delete(orderDetail);
                await _genericRepositoryOrderDetail.Save();
            }

            return true;
        }
    }
}