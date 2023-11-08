using Application.Response;
using Domain.Entities;

namespace Application.IMappers
{
    public interface ITicketMapper
    {
        Task<TicketResponse> GenerateTicketResponse(Tickets ticket, List<Guid> listaId);
        Task<CantidadTicketsResponse> GetCantidadTicketResponse(int cantidad, int contador);
    }
}