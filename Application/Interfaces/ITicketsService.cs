using Application.Request;
using Application.Response;

namespace Application.Interfaces
{
    public interface ITicketsService
    {
        public Task<TicketResponse> createTicket(int funcionId, TicketsRequest request);
    }
}
