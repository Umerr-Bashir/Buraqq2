using Buraqq.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Buraqq
{
    public class AuthorizeAdminAttribute : Attribute, IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string accessToken = context.HttpContext.Request.Cookies["user-access-token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                var db = context.HttpContext.RequestServices.GetRequiredService<BuraqqContext>();
                if (!db.Users.Where(x => x.AccessToken.Equals(accessToken) && x.Role.Name.Equals("Admin")).Any())
                    context.Result = new RedirectToRouteResult("/Account/Login");

            }
            else
                context.Result = new RedirectResult("/Account/Login");

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}    
