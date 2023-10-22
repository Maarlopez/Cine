using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Config
{
    public class PeliculasConfig : IEntityTypeConfiguration<Peliculas>
    {
        public void Configure(EntityTypeBuilder<Peliculas> entityBuilder)
        {
            entityBuilder.HasKey(p => p.PeliculaId);

            entityBuilder.Property(p => p.Titulo)
                .HasColumnName("Titulo")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(p => p.Sinopsis)
                .HasColumnName("Sinopsis")
                .HasMaxLength(255)
                .IsRequired();

            entityBuilder.Property(p => p.Poster)
                .HasColumnName("Poster")
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder.Property(p => p.Trailer)
                .HasColumnName("Trailer")
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder.HasMany(p => p.Funciones)
                .WithOne(f => f.Pelicula)
                .HasForeignKey(f => f.PeliculaId)
                .IsRequired();

            entityBuilder.HasOne(p => p.Genero)
                .WithMany(g => g.Peliculas)
                .HasForeignKey(p => p.GeneroId)
                .IsRequired();
        }
    }
}