using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPeliculasService
    {
        IEnumerable<Peliculas> GetAll();
        Peliculas GetById(int peliculaId);
        Peliculas GetByTitulo(string titulo);
        Peliculas DeletePelicula(int peliculaId);
        int GetMinId();
        int GetMaxId();
    }
}
