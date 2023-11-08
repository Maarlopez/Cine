using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class GenerosService : IGenerosService
    {
        private readonly IGenerosQuery _query;
        public GenerosService(IGenerosQuery generoQuery)
        {
            _query = generoQuery;
        }
        public async Task<List<Generos>> GetGeneros()
        {
            try
            {
                return await _query.GetGeneros();
            }
            catch (ConflictException ex)
            {
                throw new ConflictException("Error: " + ex.Message);
            }
        }
    }
}