using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PracticaMvcCore2AGC.Repositorys;
using PracticaMvcCore2AGC.Models;
using Microsoft.AspNetCore.Authorization;

namespace PracticaMvcCore2AGC.Controllers
{
    public class ManagedController : Controller
    {

        private LibrosRepository repo;

        public ManagedController(LibrosRepository repo)
        {
            this.repo = repo;
        }



        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            Usuario usu = this.repo.ComprobarUser(username, password);

            if (usu != null)
            {
                ClaimsIdentity identity =
                new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                Claim claimUserName =
                    new Claim(ClaimTypes.Name, usu.IdUsuario.ToString());
                identity.AddClaim(claimUserName);

                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.Now.AddMinutes(15)
                    });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }

            

        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Managed");
        }
    }
}

