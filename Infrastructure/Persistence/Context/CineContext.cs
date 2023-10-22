using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Persistence.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class CineContext : DbContext
    {
        public DbSet<Funciones> Funciones { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<Peliculas> Peliculas { get; set; }
        public DbSet<Salas> Salas { get; set; }
        public DbSet<Tickets> Tickets { get; set; }

        public CineContext(DbContextOptions<CineContext> options)
        : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FuncionesConfig());

            modelBuilder.ApplyConfiguration(new GenerosConfig());
            modelBuilder.ApplyConfiguration(new GeneroData());

            modelBuilder.ApplyConfiguration(new PeliculasConfig());
            modelBuilder.ApplyConfiguration(new PeliculaData());

            modelBuilder.ApplyConfiguration(new SalasConfig());
            modelBuilder.ApplyConfiguration(new SalaData());

            modelBuilder.ApplyConfiguration(new TicketsConfig());

        }
    }
}
