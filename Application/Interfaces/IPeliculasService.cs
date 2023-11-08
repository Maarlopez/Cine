using Application.Request;
using Application.Response;

namespace Application.Interfaces
{
    public interface IPeliculasService
    {
        public Task<PeliculaResponse> GetPeliculaById(int peliculaId);
        public Task<PeliculaResponse> UpdatePelicula(int peliculaId, PeliculaRequest request);
    }
}
