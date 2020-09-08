using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace GSU.Museum.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthorised = context.HttpContext.Items["IsAuthorised"] ?? false;
            if (!((bool)isAuthorised))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
