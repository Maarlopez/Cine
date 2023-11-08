using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPeliculasQuery
    {
        Task<Peliculas> GetPeliculaById(int peliculaId);
        Task<List<Peliculas>> GetPeliculas();
    }
}
