using Application.IMappers;
using Application.Response;
using Domain.Entities;

namespace Application.Mappers
{
    public class FuncionMapper : IFuncionMapper
    {
        private readonly ISalaMapper _salaMapper;
        private readonly IPeliculaMapper _peliculaMapper;
        public FuncionMapper(IPeliculaMapper peliculaMapper, ISalaMapper salaMapper)
        {
            _salaMapper = salaMapper;
            _peliculaMapper = peliculaMapper;
        }
        public async Task<FuncionResponse> GenerateFuncionResponse(Funciones funcion)
        {
            return new FuncionResponse
            {
                FuncionId = funcion.FuncionId,
                Pelicula = await _peliculaMapper.GeneratePeliculaGetResponse(funcion.Pelicula),
                Sala = await _salaMapper.GetSalaResponse(funcion.Sala),
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString("hh\\:mm"),
            };
        }

        public async Task<FuncionGetResponse> GenerateFuncionGetResponse(Funciones funcion)
        {
            return new FuncionGetResponse
            {
                FuncionId = funcion.FuncionId,
                Pelicula = await _peliculaMapper.GeneratePeliculaGetResponse(funcion.Pelicula),
                Sala = await _salaMapper.GetSalaResponse(funcion.Sala),
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString("hh\\:mm")
            };
        }

        public async Task<List<FuncionGetResponse>> GenerateListFuncionGetResponse(List<Funciones> funciones)
        {
            List<FuncionGetResponse> response = new();
            foreach (Funciones funcion in funciones)
            {
                response.Add(await GenerateFuncionGetResponse(funcion));
            }
            return response;
        }

        public async Task<FuncionDelete> GenerateFuncionDelete(Funciones funcion)
        {
            var result = new FuncionDelete
            {
                FuncionId = funcion.FuncionId,
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString("hh\\:mm")
            };
            return await Task.FromResult(result);
        }


        public async Task<List<FuncionDelete>> GenerateDeleteFunciones(ICollection<Funciones> listaFunciones)
        {
            List<FuncionDelete> funcionesDelete = new();
            foreach (Funciones funcion in listaFunciones)
            {
                funcionesDelete.Add(await GenerateFuncionDelete(funcion));

            }
            return funcionesDelete;
        }
    }
}
