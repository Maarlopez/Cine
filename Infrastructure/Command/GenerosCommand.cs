using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Command
{
    public class GenerosCommand : IGenerosCommand
    {
        private readonly CineContext _context;

        private readonly IGenerosQuery _query;

        public GenerosCommand(CineContext context, IGenerosQuery query)
        {
            _context = context;
            _query = query;
        }

        public GenerosCommand()
        {
        }
        public Generos DeleteGenero(int generoId)
        {
            Generos delete = _query.GetById(generoId);

            if (delete != null)
            {
                _context.Remove(delete);
                _context.SaveChanges();
            }
            return delete;
        }
    }
}