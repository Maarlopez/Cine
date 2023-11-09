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
                var peliculaToUpdate = await _context.Peliculas
                                                     .Include(p => p.Genero)
                                                     .FirstOrDefaultAsync(p => p.PeliculaId == peliculaId);

                if (peliculaToUpdate == null)
                {
                    throw new NotFoundException($"No se encontró una película con el Id: {peliculaId}");
                }

                // Verificar que el título no cambie si es el mismo que el original o si ya existe en otra película
                if (!string.IsNullOrEmpty(request.Titulo) && request.Titulo != peliculaToUpdate.Titulo)
                {
                    if (await _context.Peliculas.AnyAsync(p => p.Titulo == request.Titulo && p.PeliculaId != peliculaId))
                    {
                        throw new ConflictException("Ya existe una película con ese nombre.");
                    }
                    peliculaToUpdate.Titulo = request.Titulo;
                }

                // Actualiza solo los campos que no son null o no están vacíos en la solicitud PATCH
                peliculaToUpdate.Trailer = !string.IsNullOrEmpty(request.Trailer) ? request.Trailer : peliculaToUpdate.Trailer;
                peliculaToUpdate.Sinopsis = !string.IsNullOrEmpty(request.Sinopsis) ? request.Sinopsis : peliculaToUpdate.Sinopsis;

                // Actualiza el género solo si se ha proporcionado un valor válido (mayor que 0)
                if (request.Genero.HasValue && request.Genero.Value > 0)
                {
                    peliculaToUpdate.GeneroId = request.Genero.Value;
                }

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