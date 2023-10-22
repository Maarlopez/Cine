using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data
{
    public class SalaData : IEntityTypeConfiguration<Salas>
    {
        public void Configure(EntityTypeBuilder<Salas> entityBuilder)
        {
            entityBuilder.HasData
            (
                new Salas
                {
                    SalaId = 1,
                    Nombre = "2D",
                    Capacidad = 5,
                },

                new Salas
                {
                    SalaId = 2,
                    Nombre = "3D",
                    Capacidad = 15,
                },

                new Salas
                {
                    SalaId = 3,
                    Nombre = "4D",
                    Capacidad = 35,
                }
            );
        }
    }
}
