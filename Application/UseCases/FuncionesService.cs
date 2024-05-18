using Application.Exceptions;
using Application.IMappers;
using Application.Interfaces;
using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.UseCases
{
    public class FuncionesService : IFuncionesService
    {
        private readonly IFuncionesQuery _query;
        private readonly IFuncionesCommand _command;
        private readonly IPeliculasService _peliculaService;
        private readonly ISalasService _salaService;
        private readonly IFuncionMapper _funcionMapper;
        private readonly ITicketMapper _ticketMapper;

        public FuncionesService(IFuncionesCommand funcionCommand, IFuncionesQuery funcionQuery, IPeliculasService peliculaService, ISalasService salaService, IFuncionMapper funcionMapper, ITicketMapper ticketMapper)
        {
            _command = funcionCommand;
            _query = funcionQuery;
            _peliculaService = peliculaService;
            _salaService = salaService;
            _funcionMapper = funcionMapper;
            _ticketMapper = ticketMapper;
        }
        public async Task<FuncionResponse> RegisterFuncion(FuncionRequest request)
        {
            // Validación de existencia de película
            if (!await _peliculaService.PeliculaExists(request.Pelicula))
            {
                throw new PeliculaNotFoundException("No existe la película.");
            }


            // Validación de existencia de sala
            if (!await _salaService.SalaExists(request.Sala))
            {
                throw new SalaNotFoundException("No existe la sala.");
            }

             // Validación de formato de horario y rango de horas
            if (!DateTime.TryParse(request.Fecha, out DateTime date))
            {
                throw new SyntaxErrorException("Formato érroneo para la fecha, pruebe ingresando dd/mm/aaaa");
            }

            // Validación de formato de horario y rango de horas
            if (!TimeSpan.TryParse(request.Horario, out TimeSpan tiempo) || tiempo.TotalHours >= 24)
            {
                throw new SyntaxErrorException("Formato érroneo para el horario, ingrese horario desde las 00:00 a 23:59hs");
            }

            // Validación de superposición de funciones en la misma sala
            if (!await VerifyIfSalaisEmpty(DateTime.Parse(request.Fecha), tiempo, request.Sala))
            {
                throw new ConflictException("La sala ya está ocupada en esa fecha y horario.");
            }

            // Validación de fecha no anterior al día actual
            if (DateTime.Parse(request.Fecha) < DateTime.Now.Date)
            {
                throw new InvalidOperationException("No se puede registrar funciones con fechas anteriores al día actual.");
            }

            var funcion = new Funciones
            {
                PeliculaId = request.Pelicula,
                SalaId = request.Sala,
                Fecha = date,
                Horario = tiempo,
                Tickets = new List<Tickets>(),
            };

            int funcionId = await _command.InsertFuncion(funcion);
            funcion = await _query.GetFuncionById(funcionId);

            return await _funcionMapper.GenerateFuncionResponse(funcion);
        }

        public async Task<FuncionDelete> DeleteFuncion(int funcionId)
        {
            try
            {
                if (!VerifyInt(funcionId))
                {
                    throw new SyntaxErrorException();
                }
                Funciones funcion = await _query.GetFuncionById(funcionId);
                if (funcion != null)
                {
                    if (funcion.Tickets.Count() != 0)
                    {
                        throw new ConflictException("Existen tickets registrados para esa funciòn.");
                    }
                    funcion = await _command.DeleteFuncion(funcion);
                    return await _funcionMapper.GenerateFuncionDelete(funcion);
                }
                else
                {
                    throw new FuncionNotFoundException("No existe ninguna funcion con ese Id.");
                }
            }
            catch (SyntaxErrorException)
            {
                throw new SyntaxErrorException("Formato erróneo para el Id, pruebe con un entero.");
            }
            catch (FuncionNotFoundException ex)
            {
                throw new FuncionNotFoundException("Error al remover la función: " + ex.Message);
            }
            catch (ConflictException ex)
            {
                throw new ConflictException("Error al remover la función: " + ex.Message);
            }
        }

        public async Task<List<FuncionGetResponse>> GetFuncionesByTituloFechaOGenero(string? titulo, string? fecha, int genero)
        {
            DateTime date;
            List<Funciones> listFunciones = new List<Funciones>();

            if (!string.IsNullOrEmpty(fecha))
            {
                if (!DateTime.TryParse(fecha, out date))
                {
                    // Lanza la excepción directamente si el parseo falla.
                    throw new SyntaxErrorException("Formato erróneo de fecha.");
                }
                listFunciones = await _query.GetFuncionesByFecha(date);
            }

            // Añade las condiciones para titulo y genero aquí.
            if (!string.IsNullOrEmpty(titulo))
            {
                listFunciones.AddRange(await _query.GetFuncionesByTitulo(titulo));
            }

            if (genero != 0)
            {
                listFunciones.AddRange(await _query.GetFuncionByGenero(genero));
            }

            // Si no se han aplicado filtros anteriores, devuelve todas las funciones.
            if (string.IsNullOrEmpty(titulo) && genero == 0 && string.IsNullOrEmpty(fecha))
            {
                listFunciones = await _query.GetFunciones();
            }

            // Filtra duplicados si es necesario y devuelve los resultados a través del mapper.
            var filteredListFunciones = listFunciones.Distinct().ToList();
            return await _funcionMapper.GenerateListFuncionGetResponse(filteredListFunciones);
        }

        public async Task<FuncionResponse> GetFuncionResponseById(int funcionId)
        {
            if (!VerifyInt(funcionId))
            {
                throw new SyntaxErrorException("Error en la sintaxis del Id, pruebe con un entero.");
            }

            Funciones funcion = await _query.GetFuncionById(funcionId);
            if (funcion == null)
            {
                throw new ResourceNotFoundException($"No se encontró ninguna función con el Id {funcionId}.");
            }

            return await _funcionMapper.GenerateFuncionResponse(funcion);
        }

        public async Task<Funciones> GetFuncionById(int funcionId)
        {
            Funciones funcion = await _query.GetFuncionById(funcionId);
            if (funcion == null)
            {
                throw new ResourceNotFoundException($"No se encontró ninguna función con el Id {funcionId}.");
            }

            return funcion;
        }

        public async Task<CantidadTicketsResponse> GetCantidadTickets(int funcionId)
        {
            if (!VerifyInt(funcionId))
            {
                throw new SyntaxErrorException("Formato erróneo para el Id, pruebe con un entero.");
            }

            Funciones funcion = await _query.GetFuncionById(funcionId);
            if (funcion == null)
            {
                throw new FuncionNotFoundException("No se encontró ninguna función con ese Id.");
            }

            return await _ticketMapper.GetCantidadTicketResponse(funcion.Sala.Capacidad, funcion.Tickets.Count());
        }

        //Extras
        public async Task<bool> VerifyIfSalaisEmpty(DateTime fecha, TimeSpan horario, int salaId)
        {
            List<Funciones> listFunciones = await _query.GetFunciones();
            TimeSpan duracionFuncion = TimeSpan.FromHours(2) + TimeSpan.FromMinutes(30);

            var inicioNuevaFuncion = fecha + horario;
            var finNuevaFuncion = inicioNuevaFuncion + duracionFuncion;

            foreach (var funcion in listFunciones)
            {
                if (funcion.SalaId != salaId || funcion.Fecha.Date != fecha.Date)
                {
                    continue;
                }

                var inicioFuncionExistente = funcion.Fecha + funcion.Horario;
                var finFuncionExistente = inicioFuncionExistente + duracionFuncion;

                // Verifica si hay superposición
                if (inicioNuevaFuncion < finFuncionExistente && finNuevaFuncion > inicioFuncionExistente)
                {
                    return false; // Hay superposición
                }
            }
            return true; // No hay superposición
        }
        private bool VerifyInt(int entero)
        {
            // Ejemplo de validación ajustada para verificar que el ID no sea negativo
            return entero >= 0;
        }
    }
}