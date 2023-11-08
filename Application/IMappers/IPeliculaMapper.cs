using Application.Response;
using Domain.Entities;

namespace Application.IMappers
{
    public interface IPeliculaMapper
    {
        Task<PeliculaResponse> GeneratePeliculaResponse(Peliculas pelicula);
        Task<PeliculaGetResponse> GeneratePeliculaGetResponse(Peliculas pelicula);
    }
}