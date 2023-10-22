using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISalasCommand
    {
        Salas DeleteSala(int salaId);
    }
}
