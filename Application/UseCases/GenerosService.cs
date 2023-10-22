using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class GenerosService : IGenerosService
    {
        private readonly IGenerosCommand _command;
        private readonly IGenerosQuery _query;

        public GenerosService(IGenerosCommand command, IGenerosQuery query)
        {
            _command = command;
            _query = query;
        }
        public IEnumerable<Generos> GetAll()
        {
            return _query.GetAll();
        }

        public Generos GetById(int generoId)
        {
            return _query.GetById(generoId);
        }
        public Generos DeleteGenero(int generoId)
        {
            return _command.DeleteGenero(generoId);
        }
    }
}