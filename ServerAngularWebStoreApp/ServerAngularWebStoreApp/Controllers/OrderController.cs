using Common.DTOs;
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

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                //IEnumerable<OrderDTO> orders = await _orderService.GetAll();
                return Ok();
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

        [Authorize("Deliverer")]
        [HttpGet]
        [Route("complated-orders")]
        public async Task<IActionResult> GetCompletedOrders()
        {
            try
            {
                //IEnumerable<OrderDTO> orders = await _orderService.GetOrdersByStatus(Enums.OrderStatus.Done);
                return Ok();
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
        [HttpGet]
        [Route("{idPerson}/orders")]
        public async Task<IActionResult> GetOrdersByIdCustomer(int idPerson)
        {
            try
            {
                //IEnumerable<OrderDTO> orders = await _orderService.GetOrdersByIdCustomer(idPerson);
                return Ok();
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

        [Authorize("Deliverer")]
        [HttpGet]
        [Route("{idPerson}/my-orders")]
        public async Task<IActionResult> GetOrdersByIdDeliverer(int idPerson)
        {
            try
            {
                //IEnumerable<OrderDTO> orders = await _orderService.GetOrdersByIdDeliverer(idPerson);
                return Ok();
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

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetOneOrder(int id)
        {
            try
            {
                //OrderDTO dto = await _orderService.GetOne(id);
                return Ok();
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

        [Authorize("Customer,Deliverer")]
        [HttpGet]
        [Route("{IdCustomer}/current")]
        public async Task<IActionResult> CurrentOrder(int IdCustomer)
        {
            try
            {
                // OrderDTO dto = await _orderService.LastOrder(IdCustomer);
                return Ok();
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

        [Authorize]
        [HttpGet]
        [Route("order/products/{id}")]
        public async Task<IActionResult> GetProducts(int idOrder)
        {
            try
            {
                var dto = await _orderService.GetProductsFromOrder(idOrder);
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
            int orderId = -1;
            try
            {
                orderId = await _orderService.CreateOrder(request);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(orderId);
        }

        [Authorize("Customer")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> AddProduct([FromBody] OrderDetailDTO request)
        {
            int orderId = -1;
            try
            {
                orderId = await _orderService.AddProduct(request);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(orderId);
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
    }
}