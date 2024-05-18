using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFuncionesQuery
    {
        Task<Funciones> GetFuncionById(int id);
        Task<List<Funciones>> GetFunciones();
        Task<List<Funciones>> GetFuncionByGenero(int genero);
        Task<List<Funciones>> GetFuncionesByTitulo(string titulo);
        Task<List<Funciones>> GetFuncionesByFecha(DateTime fecha);
        Task<bool> VerifyIfSalaisEmpty(DateTime fecha, TimeSpan horario, int salaId);
    }
}
