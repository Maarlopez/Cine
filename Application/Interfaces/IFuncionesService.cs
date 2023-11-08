using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IFuncionesService
    {
        Task<FuncionResponse> RegisterFuncion(FuncionRequest request);
        Task<List<FuncionGetResponse>> GetFuncionesByTituloFechaOCategoria(string titulo, string fecha, int categoria);
        Task<Funciones> GetFuncionById(int funcionId);
        Task<FuncionResponse> GetFuncionResponseById(int funcionId);
        Task<FuncionDelete> DeleteFuncion(int funcionId);
        Task<CantidadTicketsResponse> GetCantidadTickets(int funcionId);
    }
}
