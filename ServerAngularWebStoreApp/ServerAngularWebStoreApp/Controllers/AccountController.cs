using Common.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using ServerAngularWebStoreApp.Authentication;
using Services.IService;

namespace ServerAngularWebStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authenticateService;

        public AccountController(IAccountService accountService, IAuthenticateService authenticateService)
        {
            _accountService = accountService;
            _authenticateService = authenticateService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(AuthenticateRequestDTO dto)
        {
            var response = await _authenticateService.Authenticate(dto);

            if (String.IsNullOrEmpty(response.Token))
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("login-google")]
        public async Task<IActionResult> LoginWithGoogle(RegisterDTO dto)
        {
            try
            {
                bool isRegistred = await _accountService.Register(dto, Enums.PersonType.Customer);

                AuthenticateRequestDTO dtoAuth = new AuthenticateRequestDTO
                {
                    Username = dto.UserName,
                    Password = ""
                };
                var response = await _authenticateService.Authenticate(dtoAuth);

                if (String.IsNullOrEmpty(response.Token))
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                return Ok(response);
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

        [HttpPost]
        [Route("register-seller")]
        public async Task<IActionResult> RegisterSeller(RegisterDTO dto)
        {
            bool isRegistered;

            try
            {
                isRegistered = await _accountService.Register(dto, Enums.PersonType.Seller);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }

            if (isRegistered)
            {
                return Ok(new { message = "User successfully register." });
            }
            else
            {
                return BadRequest(new { message = "Something went wrong." });
            }
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDTO dto)
        {
            bool isRegistered;

            try
            {
                isRegistered = await _accountService.Register(dto, Enums.PersonType.Admin);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }

            if (isRegistered)
            {
                return Ok(new { message = "User successfully register." });
            }
            else
            {
                return BadRequest(new { message = "Something went wrong." });
            }
        }

        [HttpPost]
        [Route("register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterDTO dto)
        {
            bool isRegistered;

            try
            {
                isRegistered = await _accountService.Register(dto, Enums.PersonType.Customer);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }

            if (isRegistered)
            {
                return Ok(new { message = "User successfully register." });
            }
            else
            {
                return BadRequest(new { message = "Something went wrong." });
            }
        }
    }
}