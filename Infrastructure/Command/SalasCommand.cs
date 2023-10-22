using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Command
{
    public class SalasCommand : ISalasCommand
    {
        private readonly CineContext _context;

        private readonly ISalasQuery _query;

        public SalasCommand(CineContext context, ISalasQuery query)
        {
            _context = context;
            _query = query;
        }
        public Salas DeleteSala(int salaId)
        {
            Salas delete = _query.GetById(salaId);

            if (delete != null)
            {
                _context.Remove(delete);
                _context.SaveChanges();
            }
            return delete;
        }
    }
}
