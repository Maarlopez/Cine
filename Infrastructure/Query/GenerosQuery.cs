using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Query
{
    public class GenerosQuery : IGenerosQuery
    {
        private readonly CineContext _context;

        public GenerosQuery(CineContext context)
        {
            _context = context;
        }
        public IEnumerable<Generos> GetAll()
        {
            return _context.Generos.ToList();
        }

        public Generos? GetById(int GeneroId)
        {
            return _context.Generos.FirstOrDefault(genero => genero.GeneroId == GeneroId);
        }
    }
}
