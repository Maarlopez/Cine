using Application.IMappers;
using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public class SalaMapper : ISalaMapper
    {
        public async Task<Sala> GetSalaResponse(Salas sala)
        {
            return await Task.FromResult(new Sala
            {
                Id = sala.SalaId,
                Nombre = sala.Nombre,
                Capacidad = sala.Capacidad,
            });
        }
    }
}