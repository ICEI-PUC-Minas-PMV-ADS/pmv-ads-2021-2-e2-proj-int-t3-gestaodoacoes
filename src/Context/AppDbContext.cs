using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
    }
}
