using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGenerosQuery
    {
        IEnumerable<Generos> GetAll();
        Generos? GetById(int generoId);
    }
}
