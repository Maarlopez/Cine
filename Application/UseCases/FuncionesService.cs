using Application.Interfaces;
using Application.Request;
using Application.Response;
using Domain.DTO;
using Domain.Entities;

namespace Application.UseCases
{
    public class FuncionesService : IFuncionesService
    {
        private readonly IFuncionesCommand _command;
        private readonly IFuncionesQuery _query;

        public FuncionesService(IFuncionesCommand command, IFuncionesQuery query)
        {
            _command = command;
            _query = query;
        }
        public IEnumerable<FuncionGetResponse> GetAll()
        {
            var funciones = _query.GetAll();
            return funciones.Select(f => new FuncionGetResponse
            {
                FuncionId = f.FuncionId
            }).ToList();
        }

        public FuncionResponse GetById(int funcionId)
        {
            var funcion = _query.GetById(funcionId);
            if (funcion == null)
                throw new KeyNotFoundException($"No se encontró ninguna función con el ID: {funcionId}");

            return new FuncionResponse
            {
                FuncionId = funcion.FuncionId,
                Pelicula = new PeliculaGetResponse
                {
                    PeliculaId = funcion.Pelicula.PeliculaId,
                    Titulo = funcion.Pelicula.Titulo,
                    Poster = funcion.Pelicula.Poster,
                    Genero = new Genero
                    {
                        Id = funcion.Pelicula.Genero.GeneroId,  // Asumo que Genero tiene una propiedad Id
                        Nombre = funcion.Pelicula.Genero.Nombre
                    }
                },
                Sala = new Sala
                {
                    Id = funcion.Sala.SalaId,
                    Nombre = funcion.Sala.Nombre
                },
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString(@"hh\:mm")
            };
        }

        public List<FuncionGetResponse> GetFuncionesPorFechaTituloYGenero(DateTime fecha, string titulo, string genero)
        {
            var funciones = _query.GetFuncionesPorFechaTituloYGenero(fecha, titulo, genero);

            List<FuncionGetResponse> responseList = funciones.Select(funcion => new FuncionGetResponse
            {
                FuncionId = funcion.FuncionId,
                Pelicula = new PeliculaGetResponse
                {
                    PeliculaId = funcion.Pelicula.PeliculaId,
                    Titulo = funcion.Pelicula.Titulo,
                    Poster = funcion.Pelicula.Poster,
                    Genero = new Genero
                    {
                        Id = funcion.Pelicula.Genero.GeneroId,
                        Nombre = funcion.Pelicula.Genero.Nombre
                    }
                },
                Sala = new Sala
                {
                    Id = funcion.Sala.SalaId,
                    Nombre = funcion.Sala.Nombre,
                    Capacidad = funcion.Sala.Capacidad
                },
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString(@"hh\:mm")
            }).ToList();

            return responseList;
        }

        public FuncionResponse RegistrarFuncion(FuncionRequest request)
        {
            var funcion = new Funciones
            {
                PeliculaId = request.Pelicula,
                SalaId = request.Sala,
                Fecha = request.Fecha,
                Horario = TimeSpan.Parse(request.Horario)
            };

            _command.RegistrarFuncion(funcion.PeliculaId, funcion.SalaId, funcion.Fecha, funcion.Horario);


            return new FuncionResponse
            {
                FuncionId = funcion.FuncionId,
                Pelicula = new PeliculaGetResponse
                {
                    PeliculaId = funcion.Pelicula.PeliculaId,
                    Titulo = funcion.Pelicula.Titulo,
                    Poster = funcion.Pelicula.Poster,
                    Genero = new Genero
                    {
                        Id = funcion.Pelicula.Genero.GeneroId,  // Asumo que Genero tiene una propiedad Id
                        Nombre = funcion.Pelicula.Genero.Nombre
                    }
                },
                Sala = new Sala
                {
                    Id = funcion.Sala.SalaId,
                    Nombre = funcion.Sala.Nombre
                },
                Fecha = funcion.Fecha,
                Horario = funcion.Horario.ToString(@"hh\:mm")
            };
        }
        //public FuncionDelete DeleteFuncion(int funcionId)
        //{
        //    // Suponiendo que tienes una función en _command que elimina una función y devuelve un FuncionDelete.
        //    var funcion = _command.DeleteFuncion(funcionId);

        //    return new FuncionDelete
        //    {
        //        FuncionId = funcion.FuncionId,
        //        Fecha = funcion.Fecha,
        //        Horario = funcion.Horario.ToString(@"hh\:mm")
        //    };
        //}
    }
}
