using Common.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace ServerAngularWebStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize("Customer")]
        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> OrderProduct(OrderDTO dto)
        {
            try
            {
                await _orderService.AddOrder(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize("Customer")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDetailDTO request)
        {
            OrderDetailDTO orderDetail;
            try
            {
                orderDetail = await _orderService.CreateOrder(request);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(orderDetail);
        }

        [Authorize("Customer")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> AddProduct([FromBody] OrderDetailDTO request)
        {
            OrderDetailDTO orderDetail;
            try
            {
                orderDetail = await _orderService.AddProduct(request);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(orderDetail);
        }

        [Authorize("Customer")]
        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> RemoveProduct([FromBody] OrderDetailDTO request)
        {
            bool productRemoved = false;
            try
            {
                productRemoved = await _orderService.RemoveProduct(request);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(productRemoved);
        }

        [Authorize("Customer")]
        [HttpGet]
        [Route("{idOrder}")]
        public async Task<IActionResult> GetOrder(int idOrder)
        {
            try
            {
                OrderDTO dto = await _orderService.GetOrder(idOrder);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }
        }

        [Authorize("Customer")]
        [HttpPut]
        [Route("{idOrder}")]
        public async Task<IActionResult> SaveOrder(int idOrder, [FromBody] OrderDTO dto)
        {
            bool finalizeOrder = false;
            try
            {
                finalizeOrder = await _orderService.FinalizeOrder(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(finalizeOrder);
        }

        [Authorize("Customer")]
        [HttpDelete]
        [Route("{idOrder}")]
        public async Task<IActionResult> DeleteOrder(int idOrder)
        {
            bool isDeleted = false;
            try
            {
                isDeleted = await _orderService.DeleteOrder(idOrder);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(isDeleted);
        }

        [Authorize("Customer")]
        [HttpGet("{idPerson}/orders")]
        public async Task<IActionResult> GetOrdersByIdCustomer(int idPerson, [FromQuery] int currentOrderId)
        {
            try
            {
                IEnumerable<OrderDTO> orders = await _orderService.GetOrdersByIdCustomer(idPerson, currentOrderId);
                return Ok(orders);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }
        }

        [Authorize("Customer")]
        [HttpPut]
        [Route("cancel")]
        public async Task<IActionResult> CancelOrder(OrderDTO dto)
        {
            bool orderCanceled = false;
            try
            {
                orderCanceled = await _orderService.CancelOrder(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(orderCanceled);
        }
    }
}