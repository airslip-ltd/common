using Airslip.Common.Auth.Models;
using Airslip.Common.Types.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Auth.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JwtAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string ApplicationRoles { get; set; } = string.Empty;
        public string UserRoles { get; set; } = string.Empty;
        public AirslipUserType[] AirslipUserTypes { get; set; } = Array.Empty<AirslipUserType>();
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            JsonResult fail = new(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            string[] applicationRoles = ApplicationRoles.Split(",");
            string[] userRoles = UserRoles.Split(",");
            
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            UserToken? user = (UserToken?)context.HttpContext.Items["UserToken"];
            if (user == null)
            {
                context.Result = fail;
                return;
            }
            
            if (applicationRoles.Any() && !applicationRoles.Any(o => user.ApplicationRoles.Contains(o)))
            {
                context.Result = fail;
                return;
            }
            
            if (userRoles.Any() && !userRoles.Any(o => o.Equals(user.UserRole)))
            {
                context.Result = fail;
                return;
            }
            
            if (AirslipUserTypes.Any() && !AirslipUserTypes.Any(o => o.Equals(user.AirslipUserType)))
            {
                context.Result = fail;
            }
        }
    }
}