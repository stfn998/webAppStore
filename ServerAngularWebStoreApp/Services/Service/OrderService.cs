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
        private readonly IGenericRepository<Person> _genericRepositoryPerson;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Product> genericRepositoryProduct, IGenericRepository<Order> genericRepositoryOrder, IGenericRepository<Person> genericRepositoryPerson, IGenericRepository<OrderDetail> genericRepositoryOrderDetail, IOrderRepository orderRepository, IProductRepository productRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _genericRepositoryProduct = genericRepositoryProduct;
            _genericRepositoryOrder = genericRepositoryOrder;
            _genericRepositoryOrderDetail = genericRepositoryOrderDetail;
            _genericRepositoryPerson = genericRepositoryPerson;
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

        public async Task<IEnumerable<OrderDTO>> GetOrdersByIdCustomer(int idPerson, int currentOrderId)
        {
            List<Product> products = new List<Product>();
            IEnumerable<Order> orders = await _orderRepository.GetOrdersByIdCustomer(idPerson);
            if (orders == null)
            {
                throw new KeyNotFoundException("User does not have orders.");
            }
            orders = orders.Where(o => o.Id != currentOrderId);

            IEnumerable<OrderDTO> ordersDto = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);
            foreach (OrderDTO oDto in ordersDto)
            {
                IEnumerable<OrderDetail> od = await _orderDetailRepository.GetByIdOrder(oDto.Id);
                int i = 0;
                foreach (OrderDetail odd in od)
                {
                    oDto.OrderDetails.Add(_mapper.Map<OrderDetailDTO>(odd));
                    oDto.OrderDetails[i].CustomerId = idPerson;
                    i++;
                }
            }

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

        public async Task<OrderDetailDTO> CreateOrder(OrderDetailDTO dto)
        {
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(dto);

            Order order = new Order()
            {
                CustomerId = dto.CustomerId,
            };

            if (orderDetail == null || order == null)
            {
                throw new KeyNotFoundException("Adding new order failed");
            }

            orderDetail.Quantity = 1;
            order.OrderDetails.Add(orderDetail);

            await _genericRepositoryOrder.Insert(order);
            await _genericRepositoryOrder.Save();

            Product product = await _genericRepositoryProduct.GetByObject(dto.ProductId);
            Person seller = await _genericRepositoryPerson.GetByObject(product.SellerId);

            if (product == null && seller == null)
            {
                throw new KeyNotFoundException("Adding new order failed");
            }

            product.Seller = seller;
            order.OrderDetails.FirstOrDefault().Product = product;

            orderDetail.OrderId = order.Id;
            order.ShippingCost = order.SellersShippingCosts.Values.Sum();

            await _genericRepositoryOrder.Update(order);
            await _genericRepositoryOrder.Save();

            dto = _mapper.Map<OrderDetailDTO>(orderDetail);

            dto.CustomerId = order.CustomerId;
            return dto;
        }

        public async Task<OrderDetailDTO> AddProduct(OrderDetailDTO dto)
        {
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(dto);

            Order order = await _genericRepositoryOrder.GetByObject(orderDetail.OrderId);

            if (orderDetail == null || order == null)
            {
                throw new KeyNotFoundException("Adding new order failed");
            }

            orderDetail.Quantity = 1;
            order.OrderDetails.Add(orderDetail);

            IEnumerable<OrderDetail> orderDetailList = await _orderDetailRepository.GetByIdOrder(order.Id);
            if (orderDetailList == null)
            {
                throw new KeyNotFoundException("Order does not exist");
            }

            foreach (OrderDetail orderDetail2 in orderDetailList)
            {
                order.OrderDetails.Add(orderDetail2);
            }
            int i = 0;

            foreach (OrderDetail orderDetail3 in order.OrderDetails.ToList())
            {
                Product product = await _genericRepositoryProduct.GetByObject(orderDetail3.ProductId);
                Person seller = await _genericRepositoryPerson.GetByObject(product.SellerId);

                if (product == null && seller == null)
                {
                    throw new KeyNotFoundException("Adding new order failed");
                }

                product.Seller = seller;
                order.OrderDetails.ToList()[i].Product = product;
                i++;
            }

            orderDetail.OrderId = order.Id;
            order.ShippingCost = order.SellersShippingCosts.Values.Sum();

            foreach (OrderDetail orderDetail2 in orderDetailList)
            {
                order.OrderDetails.Remove(orderDetail2);
            }

            await _genericRepositoryOrder.Update(order);
            try
            {
                await _genericRepositoryOrder.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            dto = _mapper.Map<OrderDetailDTO>(orderDetail);
            dto.CustomerId = order.CustomerId;

            return dto;
        }

        public async Task<bool> RemoveProduct(OrderDetailDTO dto)
        {
            IEnumerable<OrderDetail> orderDetailList = await _orderDetailRepository.GetByIdOrder(dto.OrderId);
            if (orderDetailList == null)
            {
                throw new KeyNotFoundException("Order does not exist");
            }

            OrderDetail orderDetail = orderDetailList.FirstOrDefault(o => o.ProductId.Equals(dto.ProductId));

            Order order = await _genericRepositoryOrder.GetByObject(orderDetail.OrderId);

            if (orderDetail == null || order == null)
            {
                throw new KeyNotFoundException("Product does not exist.");
            }

            foreach (OrderDetail orderDetail2 in orderDetailList)
            {
                order.OrderDetails.Add(orderDetail2);
            }
            int i = 0;

            foreach (OrderDetail orderDetail3 in order.OrderDetails.ToList())
            {
                Product product = await _genericRepositoryProduct.GetByObject(orderDetail3.ProductId);
                Person seller = await _genericRepositoryPerson.GetByObject(product.SellerId);

                if (product == null && seller == null)
                {
                    throw new KeyNotFoundException("Adding new order failed");
                }

                product.Seller = seller;
                order.OrderDetails.ToList()[i].Product = product;
                i++;
            }

            order.OrderDetails.Remove(orderDetail);
            orderDetailList.ToList().Remove(orderDetail);
            order.ShippingCost = order.SellersShippingCosts.Values.Sum();

            await _genericRepositoryOrderDetail.Delete(orderDetail);
            await _genericRepositoryOrderDetail.Save();

            foreach (OrderDetail orderDetail2 in orderDetailList)
            {
                order.OrderDetails.Remove(orderDetail2);
            }

            await _genericRepositoryOrder.Update(order);
            try
            {
                await _genericRepositoryOrder.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public async Task<OrderDTO> GetOrder(int idOrder)
        {
            Order order = await _genericRepositoryOrder.GetByObject(idOrder);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exist.");
            }

            OrderDTO dto = _mapper.Map<OrderDTO>(order);

            return dto;
        }

        public async Task<bool> FinalizeOrder(OrderDTO dto)
        {
            Order order = _mapper.Map<Order>(dto);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exist.");
            }

            IEnumerable<OrderDetail> orderDetailList = await _orderDetailRepository.GetByIdOrder(dto.Id);

            foreach (OrderDetail orderDetail in order.OrderDetails)
            {
                if (orderDetailList.Any(od => od.OrderId == orderDetail.OrderId && od.ProductId == orderDetail.ProductId))
                {
                    await _genericRepositoryOrderDetail.Update(orderDetail);
                    await _genericRepositoryOrderDetail.Save();
                }

                Product product = await _genericRepositoryProduct.GetByObject(orderDetail.ProductId);
                product.Quantity -= orderDetail.Quantity;

                await _genericRepositoryProduct.Update(product);
                await _genericRepositoryProduct.Save();
            }

            order.OrderDate = DateTime.Now;
            order.DeliveryTime = order.OrderDate.AddHours(new Random().Next(2, 24)); // Random delivery time greater than 1 hour from the order date

            await _genericRepositoryOrder.Update(order);
            await _genericRepositoryOrder.Save();

            return true;
        }

        public async Task<bool> DeleteOrder(int orderId)
        {
            Order order = await _genericRepositoryOrder.GetByObject(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exist.");
            }

            await _genericRepositoryOrder.Delete(order);
            await _genericRepositoryOrder.Save();

            return true;
        }

        public async Task<bool> CancelOrder(OrderDTO dto)
        {
            foreach (OrderDetailDTO orderDetail in dto.OrderDetails)
            {
                Product product = await _genericRepositoryProduct.GetByObject(orderDetail.ProductId);
                product.Quantity += orderDetail.Quantity;

                await _genericRepositoryProduct.Update(product);
                await _genericRepositoryProduct.Save();
            }

            Order order = await _genericRepositoryOrder.GetByObject(dto.Id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order does not exist.");
            }

            order.CanCancel = false;

            await _genericRepositoryOrder.Update(order);
            await _genericRepositoryOrder.Save();

            return true;
        }
    }
}