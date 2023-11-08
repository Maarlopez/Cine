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
            try
            {
                if (!VerifyInt(peliculaId)) { throw new SyntaxErrorException(); }
                Peliculas pelicula = await _query.GetPeliculaById(peliculaId);
                if (pelicula != null)
                {
                    return await _peliculaMapper.GeneratePeliculaResponse(pelicula);
                }
                else
                {
                    throw new PeliculaNotFoundException("No se encontró la película solicitada.");
                }
            }
            catch (SyntaxErrorException ex)
            {
                throw new SyntaxErrorException("Error en la sintaxis: " + ex.Message);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Error: " + ex.Message);
            }

        }
        public async Task<PeliculaResponse> UpdatePelicula(int peliculaId, PeliculaRequest request)
        {
            try
            {
                if (!VerifyInt(peliculaId))
                {
                    throw new SyntaxErrorException("Formato erróneo para el Id, pruebe con un entero.");
                }
                request.Titulo = char.ToUpper(request.Titulo[0]) + request.Titulo.Substring(1);
                Peliculas pelicula = await _query.GetPeliculaById(peliculaId);
                if (pelicula != null)
                {
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
                else
                {
                    { throw new NotFoundException("No existe ninguna película con ese Id."); }
                }
            }
            catch (SyntaxErrorException ex)
            {
                throw new SyntaxErrorException("Error en la sintaxis: " + ex.Message);
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Error: " + ex.Message);
            }
            catch (ConflictException ex)
            {
                throw new ConflictException("Error: " + ex.Message);
            }

        }

        //Private methods

        private bool VerifyInt(int entero)
        {
            return int.TryParse(entero.ToString(), out entero);
        }

        private async Task<bool> VerifySameName(string tituloPelicula, int peliculaId)
        {
            List<Peliculas> listaPeliculas = await _query.GetPeliculas();
            return (listaPeliculas.Any(p => p.Titulo.Equals(tituloPelicula) && p.PeliculaId != peliculaId));
        }

        private async Task<bool> VerifyGenero(int generoId)
        {
            List<Generos> listaGeneros = await _serviceGenero.GetGeneros();
            return (listaGeneros.Any(g => g.GeneroId == generoId));
        }
    }
}
