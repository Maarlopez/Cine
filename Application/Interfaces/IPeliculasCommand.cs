using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPeliculasCommand
    {
        Peliculas DeletePelicula(int peliculaId);
    }
}
