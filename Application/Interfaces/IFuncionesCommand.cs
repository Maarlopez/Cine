using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFuncionesCommand
    {
        Task<int> InsertFuncion(Funciones funcion);
        Task<Funciones> DeleteFuncion(Funciones funcion);
    }
}