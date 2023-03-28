using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2AGC.Models;
using PracticaMvcCore2AGC.Repositorys;

namespace PracticaMvcCore2AGC.ViewComponents
{
    public class CarritoViewComponent : ViewComponent
    {
        private LibrosRepository repo;

        public CarritoViewComponent(LibrosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sesion = HttpContext.Session.GetString("CARRITO");
            if (sesion != null)
            {
                @ViewData["CUENTA"] = this.repo.GetLibrosCarrito(sesion).Count();
            }
            else
            {
                @ViewData["CUENTA"] = 0;

            }
            return View();
        }
    }
}
