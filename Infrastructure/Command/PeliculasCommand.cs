using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Command
{
    public class PeliculasCommand : IPeliculasCommand
    {
        private readonly CineContext _context;

        private readonly IPeliculasQuery _query;

        public PeliculasCommand(CineContext context)
        {
            _context = context;
        }

        public Peliculas DeletePelicula(int peliculaId)
        {
            Peliculas delete = _query.GetById(peliculaId);

            if (delete != null)
            {
                _context.Remove(delete);
                _context.SaveChanges();
            }
            return delete;
        }
    }
}