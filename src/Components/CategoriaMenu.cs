using WebApplication2.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApplication2.Components
{
    public class CategoriaMenu : ViewComponent
    {
        private readonly ICategoriaRepository _categoriaRepository;
        public CategoriaMenu(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categorias = _categoriaRepository.Categorias.OrderBy(p => p.CategoriaNome);
            return View(categorias);
        }
    }
}