using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class SalasService : ISalasService
    {
        private readonly ISalasCommand _command;
        private readonly ISalasQuery _query;

        public SalasService(ISalasCommand command, ISalasQuery query)
        {
            _command = command;
            _query = query;
        }

        public IEnumerable<Salas> GetAll()
        {
            return _query.GetAll();
        }

        public Salas GetById(int salaId)
        {
            return _query.GetById(salaId);
        }
        public Salas DeleteSala(int salaId)
        {
            return _command.DeleteSala(salaId);
        }
        public int GetMinId()
        {
            return _query.GetAll().Min(s => s.SalaId);
        }

        public int GetMaxId()
        {
            return _query.GetAll().Max(s => s.SalaId);
        }
    }
}