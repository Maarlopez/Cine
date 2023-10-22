using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Config
{
    public class GenerosConfig : IEntityTypeConfiguration<Generos>
    {
        public void Configure(EntityTypeBuilder<Generos> entityBuilder)
        {
            entityBuilder.HasKey(g => g.GeneroId);

            entityBuilder.Property(g => g.Nombre)
                .HasColumnName("Nombre")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.HasMany(g => g.Peliculas)
                .WithOne(p => p.Genero)
                .HasForeignKey(p => p.GeneroId)
                .IsRequired();
        }
    }
}
