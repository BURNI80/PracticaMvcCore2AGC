using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2AGC.Models;
using PracticaMvcCore2AGC.Repositorys;

namespace PracticaMvcCore2AGC.ViewComponents
{
    public class MenuGenerosViewComponent : ViewComponent
    {
        private LibrosRepository repo;

        public MenuGenerosViewComponent(LibrosRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> gen = this.repo.GetGeneros();
            return View(gen);
        }
    }
}
