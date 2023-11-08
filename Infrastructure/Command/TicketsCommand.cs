using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class TicketsCommand : ITicketCommand
    {
        private readonly CineContext _context;

        public TicketsCommand(CineContext context)
        {
            _context = context;
        }

        public async Task<Guid> RegisterTicket(Tickets ticket)
        {
            try
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return ticket.TicketId;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema en añadir el ticket.");
            }
        }
    }
}