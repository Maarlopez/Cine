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
            var peliculaGetResponse = new PeliculaGetResponse
            {
                PeliculaId = pelicula.PeliculaId,
                Genero = await _generoMapper.GetGeneroMapper(pelicula.Genero)
            };

            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                peliculaGetResponse.Poster = pelicula.Poster;
            }
            if (!string.IsNullOrWhiteSpace(pelicula.Titulo))
            {
                peliculaGetResponse.Titulo = pelicula.Titulo;
            }
            return peliculaGetResponse;
        }

        public async Task<PeliculaResponse> GeneratePeliculaResponse(Peliculas pelicula)
        {
            var peliculaResponse = new PeliculaResponse
            {
                PeliculaId = pelicula.PeliculaId,
                Titulo = pelicula.Titulo,
                Genero = await _generoMapper.GetGeneroMapper(pelicula.Genero),
                Funciones = await GenerateDeleteFunciones(pelicula.Funciones),
            };

            // Añade condicionalmente campos si no están vacíos o nulos
            if (!string.IsNullOrWhiteSpace(pelicula.Sinopsis))
            {
                peliculaResponse.Sinopsis = pelicula.Sinopsis;
            }
            if (!string.IsNullOrWhiteSpace(pelicula.Poster))
            {
                peliculaResponse.Poster = pelicula.Poster;
            }
            if (!string.IsNullOrWhiteSpace(pelicula.Trailer))
            {
                peliculaResponse.Trailer = pelicula.Trailer;
            }

            return peliculaResponse;
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
