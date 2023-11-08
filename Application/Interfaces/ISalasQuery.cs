using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalasQuery
    {
        public Task<Salas> GetSalaById(int salaId);
    }
}
