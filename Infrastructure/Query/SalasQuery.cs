using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class SalasQuery : ISalasQuery
    {
        private readonly CineContext _context;

        public SalasQuery(CineContext context)
        {
            _context = context;
        }
        public async Task<Salas> GetSalaById(int salaId)
        {
            var sala = await _context.Salas.SingleOrDefaultAsync(s => s.SalaId.Equals(salaId));

            if (sala == null)
            {
                throw new NotFoundException($"No se encontró una sala con el Id: {salaId}.");
            }

            return sala;
        }
    }
}
