using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data
{
    public class GeneroData : IEntityTypeConfiguration<Generos>
    {
        public void Configure(EntityTypeBuilder<Generos> entityBuilder)
        {
            entityBuilder.HasData
            (
                new Generos
                {
                    GeneroId = 1,
                    Nombre = "Acción",
                },

                new Generos
                {
                    GeneroId = 2,
                    Nombre = "Aventuras",
                },

                new Generos
                {
                    GeneroId = 3,
                    Nombre = "Ciencia Ficción",
                },

                new Generos
                {
                    GeneroId = 4,
                    Nombre = "Comedia",
                },

                new Generos
                {
                    GeneroId = 5,
                    Nombre = "Documental",
                },

                new Generos
                {
                    GeneroId = 6,
                    Nombre = "Drama",
                },

                new Generos
                {
                    GeneroId = 7,
                    Nombre = "Fantasia",
                },

                new Generos
                {
                    GeneroId = 8,
                    Nombre = "Musical",
                },

                new Generos
                {
                    GeneroId = 9,
                    Nombre = "Suspense",
                },

                new Generos
                {
                    GeneroId = 10,
                    Nombre = "Terror",
                }
            );
        }
    }
}
