using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace ServerAngularWebStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Authorize]
        [HttpPut]
        [Route("{idPerson}")]
        public async Task<IActionResult> UpdatePerson(int idPerson, ProfileDTO dto)
        {
            try
            {
                dto.IdPerson = idPerson;
                await _personService.UpdatePerson(dto);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Authorize("Admin")]
        [HttpDelete]
        [Route("{idPerson}")]
        public async Task<IActionResult> DeletePerson(int idPerson)
        {
            try
            {
                await _personService.DeletePerson(idPerson);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Something went wrong." });
            }
            return Ok(new { message = "Person successfully deleted." });
        }

        [Authorize("Admin")]
        [HttpGet]
        [Route("sellers")]
        public async Task<IActionResult> GetSeller()
        {
            try
            {
                IEnumerable<PersonDTO> persons = await _personService.GetSeller();
                return Ok(persons);
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

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllPersons()
        {
            try
            {
                IEnumerable<PersonDTO> persons = await _personService.GetAll();
                return Ok(persons);
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
        [Route("{idPerson}")]
        public async Task<IActionResult> GetOne(int idPerson)
        {
            try
            {
                PersonDTO dto = await _personService.GetPersonById(idPerson);
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

        [Authorize]
        [HttpGet]
        [Route("{idPerson}/image")]
        public async Task<IActionResult> GetImage(int idPerson)
        {
            try
            {
                string imageUrl = await _personService.GetImage(idPerson);
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

        [Authorize]
        [HttpPost]
        [Route("{idPerson}/image")]
        public async Task<IActionResult> UploadImage(int idPerson, IFormFile file)
        {
            try
            {
                await _personService.UploadImage(idPerson, file);
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
    }
}