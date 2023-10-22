using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalasQuery
    {
        IEnumerable<Salas> GetAll();
        Salas GetById(int salaId);
    }
}
