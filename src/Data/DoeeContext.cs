using doee.Models;
using Microsoft.EntityFrameworkCore;


namespace doee.Data
{
    public class DoeeContext : DbContext
    {
        public DoeeContext(DbContextOptions<DoeeContext> options) : base(options) { }

        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }
       // public DbSet<Usuario> Usuarios { get; set; }
    }
}
