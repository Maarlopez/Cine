using Application.Request;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPeliculasCommand
    {
        Task<Peliculas> UpdatePelicula(int peliculaId, PeliculaRequest pelicula);
    }
}
