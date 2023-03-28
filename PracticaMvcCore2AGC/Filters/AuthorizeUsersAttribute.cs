using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PracticaMvcCore2AGC.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                RouteValueDictionary rutaLogin = new RouteValueDictionary
                    (
                        new { controller = "Managed", action = "Login" }
                    );
                //REDIRECCIONAMOS
                context.Result = new RedirectToRouteResult(rutaLogin);
            }
        }

    }
}
