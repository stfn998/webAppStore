using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace ServerAngularWebStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize("Seller")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddProduct(ProductDTO dto)
        {
            try
            {
                await _productService.AddProduct(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }
            return Ok(new { message = "Product successfully added." });
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                var products = await _productService.GetAllProduct();
                return Ok(products);
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

        [Authorize("Seller")]
        [HttpGet]
        [Route("seller/{idSeller}")]
        public async Task<IActionResult> GetAllProductSeller(int idSeller)
        {
            try
            {
                var products = await _productService.GetAllProductSeller(idSeller);
                return Ok(products);
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
        [Route("order/{idOrder}")]
        public async Task<IActionResult> GetProductsInOrder(int idOrder)
        {
            try
            {
                var products = await _productService.GetProductsInOrder(idOrder);
                return Ok(products);
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
        [Route("{idProduct}")]
        public async Task<IActionResult> GetProduct(int idProduct)
        {
            try
            {
                ProductDTO dto = await _productService.GetProduct(idProduct);
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

        [Authorize("Seller")]
        [HttpPut]
        [Route("{idProduct}")]
        public async Task<IActionResult> UpdateProduct(int idProduct, ProductDTO dto)
        {
            try
            {
                dto.Id = idProduct;
                await _productService.UpdateProduct(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }
            return Ok(new { message = "Product successfully updated." });
        }

        [Authorize("Seller")]
        [HttpDelete]
        [Route("{idProduct}")]
        public async Task<IActionResult> DeleteProduct(int idProduct)
        {
            try
            {
                await _productService.DeleteProduct(idProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }
            return Ok(new { message = "Product successfully deleted." });
        }

        [Authorize]
        [HttpGet]
        [Route("{idProduct}/image")]
        public async Task<IActionResult> GetImage(int idProduct)
        {
            try
            {
                string imageUrl = await _productService.GetImage(idProduct);
                if (imageUrl == null || imageUrl == "")
                {
                    return BadRequest();
                }

                var bytes = System.IO.File.ReadAllBytes(imageUrl);

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = imageUrl.Split('\\')[imageUrl.Split('\\').Length - 1],
                    Inline = false
                };

                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return File(bytes, "image/png");
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
    }
}