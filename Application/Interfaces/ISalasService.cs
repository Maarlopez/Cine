using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalasService
    {
        IEnumerable<Salas> GetAll();
        Salas GetById(int salaId);
        Salas DeleteSala(int salaId);
        int GetMinId();
        int GetMaxId();
    }
}