using B3.Infra.Data.Configuration;
using B3.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace B3.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }

        public DbSet<Titulo>? Titulos { get; set; }
        public DbSet<TipoTitulo>? TipoTitulos { get; set; }
        public DbSet<Indexador>? Indexadores { get; set; }
        public DbSet<DescontoImpostoRenda>? descontoImpostoRendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new TituloConfiguration().Configure(modelBuilder.Entity<Titulo>());
            new IndexadorConfiguration().Configure(modelBuilder.Entity<Indexador>());
            new TipoTituloConfiguration().Configure(modelBuilder.Entity<TipoTitulo>());
            new DescontoImpostoRendaConfiguration().Configure(modelBuilder.Entity<DescontoImpostoRenda>());
        }

    }
}
