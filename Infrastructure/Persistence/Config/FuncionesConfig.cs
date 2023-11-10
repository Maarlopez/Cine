using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Config
{
    public class FuncionesConfig : IEntityTypeConfiguration<Funciones>
    {
        public void Configure(EntityTypeBuilder<Funciones> entityBuilder)
        {
            entityBuilder.HasKey(f => f.FuncionId);

            entityBuilder.Property(f => f.Fecha)
                .HasColumnName("Fecha")
                .IsRequired();

            entityBuilder.Property(f => f.Horario)
                .HasColumnName("Horario")
                .IsRequired();

            entityBuilder.HasOne(f => f.Pelicula)
                .WithMany(p => p.Funciones)
                .HasForeignKey(f => f.PeliculaId)
                .IsRequired();

            entityBuilder.HasOne(f => f.Sala)
                .WithMany(s => s.Funciones)
                .HasForeignKey(f => f.SalaId)
                .IsRequired();

            entityBuilder.HasMany(f => f.Tickets)
                .WithOne(t => t.Funcion)
                .HasForeignKey(t => t.FuncionId)
                .IsRequired();
        }
    }
}