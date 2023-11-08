using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalasService
    {
        public Task<bool> SalaExists(int salaId);
        public Task<Salas> GetSalaById(int salaId);
    }
}