using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class GenerosQuery : IGenerosQuery
    {
        private readonly CineContext _context;

        public GenerosQuery(CineContext context)
        {
            _context = context;
        }

        public async Task<List<Generos>> GetGeneros()
        {
            try
            {
                List<Generos> todosLosGeneros = await _context.Generos
                .ToListAsync();

                return todosLosGeneros;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema al obtener funciones.");
            }
        }
    }
}