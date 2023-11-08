using Application.Response;
using Domain.Entities;

namespace Application.IMappers
{
    public interface ISalaMapper
    {
        Task<Sala> GetSalaResponse(Salas sala);
    }
}