using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class SalasService : ISalasService
    {
        private readonly ISalasQuery _query;

        public SalasService(ISalasQuery salaQuery)
        {
            _query = salaQuery;
        }
        public async Task<bool> SalaExists(int salaId)
        {
            return await GetSalaById(salaId) != null;
        }
        public async Task<Salas> GetSalaById(int salaId)
        {
            return await _query.GetSalaById(salaId);
        }
    }
}