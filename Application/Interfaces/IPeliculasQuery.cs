using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPeliculasQuery
    {
        IEnumerable<Peliculas> GetAll();
        Peliculas GetById(int peliculaId);
        Peliculas GetByTitulo(string titulo);
    }
}
