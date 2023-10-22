using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGenerosCommand
    {
        Generos DeleteGenero(int generoId);
    }
}
