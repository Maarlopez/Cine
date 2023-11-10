using Application.Response;
using Domain.Entities;

namespace Application.IMappers
{
    public interface IFuncionMapper
    {
        Task<FuncionGetResponse> GenerateFuncionGetResponse(Funciones funcion);
        Task<FuncionResponse> GenerateFuncionResponse(Funciones funcion);
        Task<FuncionDelete> GenerateFuncionDelete(Funciones funcion);
        Task<List<FuncionDelete>> GenerateDeleteFunciones(ICollection<Funciones> listaFunciones);
        Task<List<FuncionGetResponse>> GenerateListFuncionGetResponse(List<Funciones> funciones);
    }
}