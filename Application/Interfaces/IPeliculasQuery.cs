using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPeliculasQuery
    {
        Task<Peliculas> GetPeliculaById(int peliculaId);
        Task<List<Peliculas>> GetPeliculas();
        Task<bool> PeliculaExists(int peliculaId);
        Task<bool> VerifySameName(string tituloPelicula, int peliculaId);
        Task<bool> VerifyGenero(int generoId);
    }
}
