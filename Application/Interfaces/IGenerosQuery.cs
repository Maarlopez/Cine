using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGenerosQuery
    {
        Task<List<Generos>> GetGeneros();
    }
}
