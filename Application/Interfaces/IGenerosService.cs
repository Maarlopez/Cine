using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGenerosService
    {
        IEnumerable<Generos> GetAll();
        Generos GetById(int generoId);
        Generos DeleteGenero(int generoId);
    }
}