using Application.IMappers;
using Application.Response;
using Domain.Entities;

namespace Application.Mappers
{
    public class PeliculaMapper : IPeliculaMapper
    {
        private readonly IGeneroMapper _generoMapper;


        public PeliculaMapper(IGeneroMapper generoMapper)
        {
            _generoMapper = generoMapper;
        }

        public async Task<PeliculaGetResponse> GeneratePeliculaGetResponse(Peliculas pelicula)
        {
            var result = new PeliculaGetResponse
            {
                PeliculaId = pelicula.PeliculaId,
                Titulo = pelicula.Titulo,
                Poster = pelicula.Poster,
                Genero = await _generoMapper.GetGeneroMapper(pelicula.Genero)
            };
            return await Task.FromResult(result);
        }
        public async Task<PeliculaResponse> GeneratePeliculaResponse(Peliculas pelicula)
        {
            return new PeliculaResponse
            {
                PeliculaId = pelicula.PeliculaId,
                Titulo = pelicula.Titulo,
                Sinopsis = pelicula.Sinopsis,
                Poster = pelicula.Poster,
                Trailer = pelicula.Trailer,
                Genero = await _generoMapper.GetGeneroMapper(pelicula.Genero),
                Funciones = await GenerateDeleteFunciones(pelicula.Funciones),
            };
        }

        private async Task<FuncionDelete> GenerateFuncionDelete(Funciones funcion)
        {
            var result = new FuncionDelete
            {
                FuncionId = funcion.FuncionId,
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString("hh\\:mm")
            };
            return await Task.FromResult(result);
        }
        private async Task<List<FuncionDelete>> GenerateDeleteFunciones(ICollection<Funciones> listaFunciones)
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
