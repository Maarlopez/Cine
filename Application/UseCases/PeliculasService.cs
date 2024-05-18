using Application.Exceptions;
using Application.IMappers;
using Application.Interfaces;
using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.UseCases
{
    public class PeliculasService : IPeliculasService
    {
        private readonly IPeliculasQuery _query;
        private readonly IPeliculasCommand _command;
        private readonly IGenerosService _serviceGenero;
        private readonly IPeliculaMapper _peliculaMapper;

        public PeliculasService(IPeliculasQuery peliculaQuery, IPeliculasCommand peliculaCommand, IGenerosService generoService, IPeliculaMapper peliculaMapper)
        {
            _query = peliculaQuery;
            _command = peliculaCommand;
            _serviceGenero = generoService;
            _peliculaMapper = peliculaMapper;
        }
        public async Task<PeliculaResponse> GetPeliculaById(int peliculaId)
        {
            if (!VerifyInt(peliculaId))
            {
                throw new SyntaxErrorException("Error en la sintaxis del Id, pruebe con un entero.");
            }
            Peliculas pelicula = await _query.GetPeliculaById(peliculaId);
            if (pelicula == null)
            {
                throw new PeliculaNotFoundException("No se encontró la película solicitada.");
            }
            return await _peliculaMapper.GeneratePeliculaResponse(pelicula);
        }

        public async Task<PeliculaResponse> UpdatePelicula(int peliculaId, PeliculaRequest request)
        {
            if (!VerifyInt(peliculaId))
            {
                throw new SyntaxErrorException("Formato erróneo para el Id, pruebe con un entero.");
            }

            Peliculas pelicula = await _query.GetPeliculaById(peliculaId);
            if (pelicula == null)
            {
                throw new NotFoundException("No existe ninguna película con ese Id.");
            }

            if (await VerifySameName(request.Titulo, peliculaId))
            {
                throw new ConflictException("Ya existe una película con ese nombre.");
            }

            if (!await VerifyGenero(request.Genero))
            {
                throw new NotFoundException("No existe ningún género con ese Id.");
            }

            pelicula = await _command.UpdatePelicula(peliculaId, request);
            return await _peliculaMapper.GeneratePeliculaResponse(pelicula);
        }

        //Extra methods

        private bool VerifyInt(int entero)
        {
            return entero >= 0;
        }
        public async Task<bool> VerifySameName(string tituloPelicula, int peliculaId)
        {
            List<Peliculas> listaPeliculas = await _query.GetPeliculas() ?? new List<Peliculas>();  // Asegura que listaPeliculas no sea null
            return listaPeliculas.Any(p => p.Titulo.Equals(tituloPelicula, StringComparison.OrdinalIgnoreCase) && p.PeliculaId != peliculaId);
        }
        public async Task<bool> VerifyGenero(int generoId)
        {
            // Asegúrate de que el servicio de género está inicializado correctamente
            if (_serviceGenero == null)
            {
                throw new InvalidOperationException("El servicio de géneros no está disponible.");
            }

            // Intenta obtener la lista de géneros y maneja el caso donde la lista pueda ser null
            List<Generos> listaGeneros = await _serviceGenero.GetGeneros() ?? new List<Generos>();
            return listaGeneros.Any(g => g.GeneroId == generoId);
        }

        public async Task<bool> PeliculaExists(int peliculaId)
        {
            return await _query.PeliculaExists(peliculaId);
        }
    }
}
