using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2AGC.Extensions;
using PracticaMvcCore2AGC.Filters;
using PracticaMvcCore2AGC.Models;
using PracticaMvcCore2AGC.Repositorys;
using System.Diagnostics;
using System.Security;

namespace PracticaMvcCore2AGC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LibrosRepository repo;

        public HomeController(ILogger<HomeController> logger, LibrosRepository repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        public IActionResult Index(int id, int? pos)
        {
            int numLibros = this.repo.CountLibros();
            if(pos == null)
            {
                pos = 0;
            }
            if(pos > numLibros)
            {
                pos = 0;
            }
            if(pos < 0)
            {
                pos = 0;
            }
            ViewData["ANTERIOR"] = pos - 5;
            ViewData["SIGUIENTE"] = pos + 5;
            if (id == 0)
            {
                return View(this.repo.GetLibros((int)pos));
            }
            else
            {
                return View(this.repo.GetLibrosGenero(id));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Libro(int id)
        {
            return View(this.repo.GetLibro(id));
        }

        [HttpPost]
        public IActionResult Libro(string idLibro, string titulo)
        {
            if (HttpContext.Session.GetString("CARRITO") == null)
            {
                HttpContext.Session.SetString("CARRITO", idLibro);
            }
            else
            {
                string carrito = HttpContext.Session.GetString("CARRITO");
                HttpContext.Session.SetString("CARRITO", carrito + "," + idLibro);
            }
            return View(this.repo.GetLibro(int.Parse(idLibro)));
        }

        public IActionResult Carrito()
        {
            string sesion = HttpContext.Session.GetString("CARRITO");
            return View(this.repo.GetLibrosCarrito(sesion));
        }

        [HttpPost]
        public IActionResult Carrito(int posicion)
        {
            string sesion = HttpContext.Session.GetString("CARRITO");
            HttpContext.Session.SetString("CARRITO", this.repo.DeleteLibrosCarrito(sesion, posicion));
            string sesiondef = HttpContext.Session.GetString("CARRITO");
            return View(this.repo.GetLibrosCarrito(sesiondef));
        }

        [AuthorizeUsers]
        public IActionResult Perfil()
        {
            return View(this.repo.GetPerfil(int.Parse(HttpContext.User.Identity.Name)));
        }

        [AuthorizeUsers]
        public async Task<IActionResult> Comprar()
        {
            string sesion = HttpContext.Session.GetString("CARRITO");
            await this.repo.Comprar(sesion, int.Parse(HttpContext.User.Identity.Name));
            HttpContext.Session.Remove("CARRITO");
            return View(this.repo.GetVistaPedidos(int.Parse(HttpContext.User.Identity.Name)));
        }
    }
}