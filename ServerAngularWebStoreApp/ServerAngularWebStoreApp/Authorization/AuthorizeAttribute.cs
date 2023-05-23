using Common.Models;
using DataAcceess.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServerAngularWebStoreApp.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly String[] _roles;

        public AuthorizeAttribute(params String[] roles)
        {
            _roles = roles ?? new String[] { };
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var _personRepository = (IPersonRepository)context.HttpContext.RequestServices.GetService(typeof(IPersonRepository));
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            Person person = new Person();
            var user = (User)context.HttpContext.Items["User"];
            String[] roleArray;
            if (_roles.Any())
            {
                roleArray = _roles[0].Split(',').Select(p => p.Trim()).ToArray();
            }
            else
            {
                roleArray = new String[0];
            }

            if (user != null)
            {
                person = _personRepository.GetPersonByUserId(user.Id).Result;
            }

            if (user == null || (roleArray.Any() && !roleArray.Contains(person.PersonType.ToString())))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}