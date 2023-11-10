using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class TicketQuery : ITicketQuery
    {
        private readonly CineContext _context;

        public TicketQuery(CineContext context)
        {
            _context = context;
        }
        public async Task<Tickets> GetTicketById(Guid id)
        {
            try
            {
                var ticket = await _context.Tickets
                    .Include(t => t.Funcion)
                    .Include(t => t.Funcion.Sala)
                    .Include(t => t.Funcion.Pelicula)
                    .Include(t => t.Funcion.Pelicula.Genero)
                    .SingleOrDefaultAsync(t => t.TicketId.Equals(id));

                if (ticket == null)
                {
                    throw new NotFoundException($"No se encontró un ticket con el Id: {id}.");
                }

                return ticket;
            }
            catch (DbUpdateException)
            {
                throw new SyntaxErrorException("Error en la base de datos: Problema con el ticket.");
            }
        }
    }
}