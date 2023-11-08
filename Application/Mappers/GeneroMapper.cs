using Application.IMappers;
using Application.Response;
using Domain.Entities;

namespace Application.Mappers
{
    public class GeneroMapper : IGeneroMapper
    {
        public async Task<Genero> GetGeneroMapper(Generos genero)
        {
            var result = new Genero
            {
                Id = genero.GeneroId,
                Nombre = genero.Nombre
            };
            return await Task.FromResult(result);
        }
    }
}
