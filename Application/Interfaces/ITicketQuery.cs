using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITicketQuery
    {
        Task<Tickets> GetTicketById(Guid ticketId);
    }
}