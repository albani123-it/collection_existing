using Collectium.Model.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Collectium.Config
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JWTAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _roles;

        public JWTAuthorizeAttribute(params string[] roles)
        {
            _roles = roles ?? new string[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }
            if (_roles.Any() == false)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized::No Role" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            // authorization
            var user = context.HttpContext.Items["User"] as User;
            if (user == null || user.Role == null || (!_roles.Contains(item: user.Role.Name!)))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized::Not match" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
