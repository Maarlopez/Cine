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

        public IEnumerable<Funciones> GetAll()
        {
            return _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.Tickets);
        }

        public Funciones GetById(int funcionId)
        {
            var funcion = _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.Tickets)
                .FirstOrDefault(f => f.FuncionId == funcionId);

            return funcion;
        }
        public List<Funciones> GetFuncionesPorFechaTituloYGenero(DateTime fecha, string titulo, string genero)
        {
            // Verificar si el género existe
            var generoExists = _context.Generos.Any(g => g.Nombre == genero);
            if (!generoExists)
            {
                throw new Exception("El género no existe.");
            }

            // Verificar si el título de la película existe
            var peliculaExists = _context.Peliculas.Any(p => p.Titulo == titulo);
            if (!peliculaExists)
            {
                throw new Exception("La película no existe.");
            }

            // Consulta base para obtener Funciones
            var query = _context.Funciones
                        .Include(f => f.Pelicula)
                        .Include(f => f.Sala)
                        .Include(f => f.Tickets)
                        .Where(f => f.Pelicula.Titulo == titulo && f.Pelicula.Genero.Nombre == genero);

            // Si se proporciona una fecha que no es el valor mínimo, filtrar por ella
            if (fecha != DateTime.MinValue)
            {
                query = query.Where(f => f.Fecha == fecha);
            }

            // Ejecutar la consulta y retornar los resultados
            var funcionesList = query.ToList();

            return funcionesList;
        }
    }
}
