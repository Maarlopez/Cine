using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Query
{
    public class SalasQuery : ISalasQuery
    {
        private readonly CineContext _context;

        public SalasQuery(CineContext context)
        {
            _context = context;
        }
        public IEnumerable<Salas> GetAll()
        {
            return _context.Salas;
        }

        public Salas GetById(int SalaId)
        {
            return _context.Salas.FirstOrDefault(sala => sala.SalaId == SalaId);
        }
    }
}
