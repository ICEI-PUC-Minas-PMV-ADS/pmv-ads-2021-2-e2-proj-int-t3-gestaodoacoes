using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> Categorias { get; }
    }
}
