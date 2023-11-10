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
            var pelicula = await _peliculaService.GetPeliculaById(request.Pelicula);
            if (pelicula == null)
            {
                throw new PeliculaNotFoundException("No existe la película.");
            }

            // Validación de existencia de sala
            if (!await _salaService.SalaExists(request.Sala))
            {
                throw new SalaNotFoundException("No existe la sala.");
            }

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
            try
            {
                DateTime date;
                List<Funciones> funcionesByFecha = new();
                List<Funciones> funcionesByTitulo = new();
                List<Funciones> listFunciones = new();
                List<Funciones> funcionesByGenero = new();

                if (titulo == null && fecha == null && genero == 0)
                {
                    listFunciones = await _query.GetFunciones();
                }
                if (fecha != null)
                {
                    if (!DateTime.TryParse(fecha, out date))
                    {
                        throw new SyntaxErrorException("Formato érroneo de fecha.");
                    }

                    listFunciones = await _query.GetFuncionesByFecha(date);

                    if (listFunciones.Count() == 0 && titulo != "")
                    {
                        return await _funcionMapper.GenerateListFuncionGetResponse(listFunciones);
                    }
                }
                if (titulo != null)
                {
                    funcionesByTitulo = await _query.GetFuncionesByTitulo(titulo);
                    if (listFunciones.Count() > 0)
                    { listFunciones = GroupData(listFunciones, funcionesByTitulo); }
                    else
                    { listFunciones = funcionesByTitulo; }
                }
                if (genero != 0)
                {
                    funcionesByGenero = await _query.GetFuncionByGenero(genero);
                    if (listFunciones.Count() > 0)
                    {
                        listFunciones = GroupData(listFunciones, funcionesByGenero);
                    }
                    else
                    {
                        listFunciones = funcionesByGenero;
                    }

                }
                return await _funcionMapper.GenerateListFuncionGetResponse(listFunciones);
            }
            catch (SyntaxErrorException ex)
            {
                throw new SyntaxErrorException("Error en la sintaxis ingresada para la fecha: " + ex.Message);
            }
        }

        public async Task<FuncionResponse> GetFuncionResponseById(int funcionId)
        {
            try
            {
                if (!VerifyInt(funcionId)) { throw new SyntaxErrorException(); }
                Funciones funcion = await GetFuncionById(funcionId);
                return await _funcionMapper.GenerateFuncionResponse(funcion);
            }
            catch (ResourceNotFoundException ex)
            {
                throw new ResourceNotFoundException(ex.Message);
            }
            catch (SyntaxErrorException)
            {
                throw new SyntaxErrorException("Error en la sintaxis del Id, pruebe con un entero.");
            }
        }

        public async Task<Funciones> GetFuncionById(int funcionId)
        {
            try
            {
                Funciones funcion = await _query.GetFuncionById(funcionId);
                if (funcion != null)
                {
                    return funcion;
                }
                else
                {
                    throw new SyntaxErrorException("No existe funcion con ese Id.");
                }
            }
            catch (ResourceNotFoundException ex)
            {
                throw new ResourceNotFoundException("Error en la búsqueda: " + ex.Message);
            }
        }

        public async Task<CantidadTicketsResponse> GetCantidadTickets(int funcionId)
        {
            try
            {
                if (!VerifyInt(funcionId))
                {
                    throw new SyntaxErrorException("Formato erróneo para el Id, pruebe con un entero.");
                }
                Funciones funcion = await _query.GetFuncionById(funcionId);
                if (funcion != null)
                {
                    return await _ticketMapper.GetCantidadTicketResponse(funcion.Sala.Capacidad, funcion.Tickets.Count());
                }
                else
                {
                    throw new FuncionNotFoundException("No se encontró ninguna función con ese Id.");
                }
            }
            catch (SyntaxErrorException ex)
            {
                throw new SyntaxErrorException("Error en la sintaxis: " + ex.Message);
            }
            catch (ResourceNotFoundException ex)
            {
                throw new ResourceNotFoundException("Error en la búsqueda: " + ex.Message);
            }

        }

        private List<Funciones> GroupData(List<Funciones> listaPrincipal, List<Funciones> listaSecundaria)
        {
            List<Funciones> lista = new();
            foreach (Funciones funcion in listaSecundaria)
            {
                if (listaPrincipal.Any(f => f.FuncionId == funcion.FuncionId && f.Fecha.Date == funcion.Fecha.Date && f.Pelicula.GeneroId == funcion.Pelicula.GeneroId))
                { lista.Add(funcion); }
            }
            return lista;
        }

        //Extras
        private async Task<bool> VerifyIfSalaisEmpty(DateTime fecha, TimeSpan horario, int salaId)
        {
            List<Funciones> listFunciones = await _query.GetFunciones();
            TimeSpan LapsoHorario = TimeSpan.FromHours(2) + TimeSpan.FromMinutes(30);
            return !(listFunciones.Any(f =>
               (f.SalaId == salaId && f.Fecha == fecha) &&
               (Math.Abs((horario - f.Horario).Ticks) <= LapsoHorario.Ticks ||
                Math.Abs((f.Horario - horario).Ticks) <= LapsoHorario.Ticks)));
        }
        private bool VerifyInt(int entero)
        {
            return int.TryParse(entero.ToString(), out entero);
        }
    }
}