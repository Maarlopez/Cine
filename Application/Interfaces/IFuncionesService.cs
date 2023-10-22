using Application.Request;
using Application.Response;

namespace Application.Interfaces
{
    public interface IFuncionesService
    {
        IEnumerable<FuncionGetResponse> GetAll();
        FuncionResponse GetById(int funcionId);
        List<FuncionGetResponse> GetFuncionesPorFechaTituloYGenero(DateTime fecha, string titulo, string genero);
        FuncionResponse RegistrarFuncion(FuncionRequest request);
        //FuncionDelete DeleteFuncion(int funcionId);
    }
}
