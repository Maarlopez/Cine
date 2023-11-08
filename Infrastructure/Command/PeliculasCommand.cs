using Application.Exceptions;
using Application.Interfaces;
using Application.Request;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class PeliculasCommand : IPeliculasCommand
    {
        private readonly CineContext _context;

        public PeliculasCommand(CineContext context)
        {
            _context = context;
        }

        public async Task<Peliculas> UpdatePelicula(int peliculaId, PeliculaRequest request)
        {
            try
            {
                var peliculaToUpdate = await _context.Peliculas.FirstOrDefaultAsync(p => p.PeliculaId == peliculaId);

                if (peliculaToUpdate == null)
                {
                    throw new NotFoundException($"No se encontró una película con el Id: {peliculaId}");
                }

                peliculaToUpdate.Titulo = request.Titulo;
                peliculaToUpdate.Trailer = request.Trailer;
                peliculaToUpdate.Sinopsis = request.Sinopsis;
                peliculaToUpdate.GeneroId = request.Genero;
                await _context.SaveChangesAsync();
                return peliculaToUpdate;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Hubo un error en la base de datos");
            }
        }
    }
}