using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFuncionesQuery
    {
        IEnumerable<Funciones> GetAll();
        Funciones GetById(int funcionId);
        List<Funciones> GetFuncionesPorFechaTituloYGenero(DateTime fecha, string titulo, string genero);
    }
}
