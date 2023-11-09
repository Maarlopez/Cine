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
                if (!VerifyInt(peliculaId)) { 
                    throw new SyntaxErrorException(); 
                }
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

                // Convertimos el primer carácter del título a mayúscula.
                request.Titulo = char.ToUpper(request.Titulo[0]) + request.Titulo.Substring(1);

                // Obtenemos la película por ID para verificar si necesitamos actualizarla.
                Peliculas pelicula = await _query.GetPeliculaById(peliculaId);
                if (pelicula == null)
                {
                    throw new NotFoundException("No existe ninguna película con ese Id.");
                }

                // Verificar si el nombre ya existe y si es diferente al nombre actual de la película.
                if (pelicula.Titulo != request.Titulo && await VerifySameName(request.Titulo, peliculaId))
                {
                    throw new ConflictException("Ya existe una película con ese nombre.");
                }

                // Verificar que el género exista si se proporciona un nuevo género.
                if (request.Genero.HasValue && request.Genero.Value > 0 && !await VerifyGenero(request.Genero.Value))
                {
                    throw new NotFoundException("No existe ningún género con ese Id.");
                }

                // Actualizar la película con el método de comando que maneja las propiedades de manera condicional.
                pelicula = await _command.UpdatePelicula(peliculaId, request);

                // Mapear la película actualizada a PeliculaResponse.
                return await _peliculaMapper.GeneratePeliculaResponse(pelicula);
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
            // Convertimos el entero a string y luego comprobamos que no tenga espacios
            var enteroString = entero.ToString();
            if (string.IsNullOrWhiteSpace(enteroString) || enteroString.Any(char.IsWhiteSpace))
            {
                throw new SyntaxErrorException("Formato erróneo para el Id, pruebe con un entero.");
            }
            // Si no es un número o contiene caracteres no deseados, retornamos false
            if (!int.TryParse(enteroString, out _) || enteroString.Any(ch => !char.IsDigit(ch)))
            {
                throw new SyntaxErrorException("Formato erróneo para el Id, pruebe con un entero.");
            }
            return true;
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
