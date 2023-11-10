using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Config
{
    public class SalasConfig : IEntityTypeConfiguration<Salas>
    {
        public void Configure(EntityTypeBuilder<Salas> entityBuilder)
        {
            entityBuilder.HasKey(s => s.SalaId);

            entityBuilder.Property(s => s.Nombre)
                .HasColumnName("Nombre")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.Property(s => s.Capacidad)
                .HasColumnName("Capacidad")
                .IsRequired();

            entityBuilder.HasMany(s => s.Funciones)
                .WithOne(f => f.Sala)
                .HasForeignKey(f => f.SalaId)
                .IsRequired();
        }
    }
}