using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class FuncionesQuery : IFuncionesQuery
    {
        private readonly CineContext _context;

        public FuncionesQuery(CineContext context)
        {
            _context = context;
        }
        public async Task<List<Funciones>> GetFuncionesByFecha(DateTime fecha)
        {
            try
            {
                List<Funciones> FuncionesPorFecha = await _context.Funciones
                    .Include(f => f.Pelicula)
                    .Include(f => f.Pelicula.Genero)
                    .Include(f => f.Sala)
                    .Where(f => f.Fecha.Date == fecha.Date)
                    .ToListAsync();
                return FuncionesPorFecha;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema con la fecha.");
            }
        }

        public async Task<List<Funciones>> GetFuncionesByTitulo(string titulo)
        {
            try
            {
                List<Funciones> FuncionesPorTitulo = await _context.Funciones
               .Include(p => p.Pelicula)
               .Include(f => f.Pelicula.Genero)
               .Include(f => f.Sala)
               .Where(p => p.Pelicula.Titulo.Contains(titulo))
               .ToListAsync();
                return FuncionesPorTitulo;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema con el titulo.");
            }
        }
        public async Task<List<Funciones>> GetFunciones()
        {
            try
            {
                List<Funciones> funciones = await _context.Funciones
                .Include(f => f.Sala)
                .Include(f => f.Pelicula)
                .Include(f => f.Pelicula.Genero)
                .Include(f => f.Tickets)
                .ToListAsync();

                return funciones;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema al obtener funciones");
            }
        }
        public async Task<Funciones> GetFuncionById(int id)
        {
            try
            {
                var funcion = await _context.Funciones
                    .Include(f => f.Sala)
                    .Include(f => f.Pelicula)
                    .Include(f => f.Pelicula.Genero)
                    .Include(f => f.Tickets)
                    .SingleOrDefaultAsync(f => f.FuncionId == id);

                if (funcion == null)
                {
                    throw new NotFoundException($"No se encontró una función con el Id: {id}");
                }

                return funcion;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema al obtener una función.");
            }
        }
        public async Task<List<Funciones>> GetFuncionByGenero(int generoId)
        {
            try
            {
                List<Funciones> funcionesPorGenero = await _context.Funciones
               .Include(p => p.Pelicula)
               .Include(f => f.Pelicula.Genero)
               .Include(f => f.Sala)
               .Where(p => p.Pelicula.GeneroId == generoId)
               .ToListAsync();
                return funcionesPorGenero;
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Error en la base de datos: Problema con el titulo.");
            }
        }
    }
}
