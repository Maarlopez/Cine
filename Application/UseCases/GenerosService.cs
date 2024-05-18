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

        public async Task<bool> VerifyGenero(int generoId)
        {
            // Asegúrate de que el servicio de género está inicializado correctamente
            if (_query == null)
            {
                throw new InvalidOperationException("El servicio de géneros no está disponible.");
            }

            // Intenta obtener la lista de géneros y maneja el caso donde la lista pueda ser null
            List<Generos> listaGeneros = await _query.GetGeneros() ?? new List<Generos>();
            return listaGeneros.Any(g => g.GeneroId == generoId);
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