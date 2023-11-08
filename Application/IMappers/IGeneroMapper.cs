using Application.Response;
using Domain.Entities;

namespace Application.IMappers
{
    public interface IGeneroMapper
    {
        Task<Genero> GetGeneroMapper(Generos genero);
    }
}