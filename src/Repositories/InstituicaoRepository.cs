using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApplication2.Models;
using WebApplication2.Repositories.Interfaces;

namespace WebApplication2.Repositories
{
    public class InstituicaoRepository : IInstituicaoRepository
    {
        private readonly AppDbContext _context;

        public InstituicaoRepository(AppDbContext contexto)
        {
            _context = contexto;
        }

        public IEnumerable<Instituicao> Instituicoes => _context.Instituicoes.Include(c => c.Categoria);


        public Instituicao GetInstituicaoById(string cnpj) => _context.Instituicoes.FirstOrDefault(l => l.CNPJ == cnpj);
    }
}
