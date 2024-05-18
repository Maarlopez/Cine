using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class PeliculasQuery : IPeliculasQuery
{
    private readonly CineContext _context;

    public PeliculasQuery(CineContext context)
    {
        _context = context;
    }

    public async Task<Peliculas> GetPeliculaById(int peliculaId)
    {
        try
        {
            var pelicula = await _context.Peliculas
                        .Include(p => p.Genero)
                        .Include(p => p.Funciones)
                        .SingleOrDefaultAsync(p => p.PeliculaId == peliculaId);

            if (pelicula == null)
            {
                throw new NotFoundException($"No se encontró una película con el Id: {peliculaId}.");
            }

            return pelicula;
        }
        catch (DbUpdateException)
        {
            throw new ConflictException("Error en la base de datos: Problema al obtener las películas.");
        }
    }

    public async Task<List<Peliculas>> GetPeliculas()
    {
        try
        {
            List<Peliculas> todasLasFunciones = await _context.Peliculas
            .Include(p => p.Genero)
            .Include(p => p.Funciones)
            .ToListAsync();

            return todasLasFunciones;
        }
        catch (DbUpdateException)
        {
            throw new ConflictException("Error en la base de datos: Problema al obtener las películas.");
        }
    }

    public async Task<bool> PeliculaExists(int peliculaId)
    {
        return await _context.Peliculas.AnyAsync(p => p.PeliculaId == peliculaId);
    }

    public async Task<bool> VerifyGenero(int generoId)
    {
        return await _context.Generos.AnyAsync(g => g.GeneroId == generoId);
    }

    public async Task<bool> VerifySameName(string tituloPelicula, int peliculaId)
    {
        return await _context.Peliculas.AnyAsync(p => p.Titulo.ToLower() == tituloPelicula.ToLower() && p.PeliculaId != peliculaId);
    }
}