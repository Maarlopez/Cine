using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITicketCommand
    {
        Task<Guid> RegisterTicket(Tickets ticket);
    }
}
