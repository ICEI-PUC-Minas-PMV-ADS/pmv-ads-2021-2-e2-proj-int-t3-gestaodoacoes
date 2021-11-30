using System.Collections.Generic;
using WebApplication2.Models;
using WebApplication2.Repositories.Interfaces;

namespace WebApplication2.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext contexto)
        {
            _context = contexto;
        }
        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}
