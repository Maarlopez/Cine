using Application.Exceptions;
using Application.IMappers;
using Application.Interfaces;
using Application.Mappers;
using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.UseCases
{
    public class TicketsService : ITicketsService
    {
        private readonly ITicketCommand _command;
        private readonly ITicketQuery _query;
        private readonly IFuncionesService _funcionService;
        private readonly IPeliculaMapper _peliculaMapper;
        private readonly ISalaMapper _salaMapper;
        private readonly ITicketMapper _ticketMapper;

        public TicketsService(ITicketCommand command, ITicketQuery query, IFuncionesService funcionService, IPeliculaMapper peliculaMapper, ISalaMapper salaMapper, ITicketMapper ticketMapper)
        {
            _command = command;
            _query = query;
            _funcionService = funcionService;
            _peliculaMapper = peliculaMapper;
            _salaMapper = salaMapper;
            _ticketMapper = ticketMapper;
        }

        public async Task<TicketResponse> createTicket(int funcionId, TicketsRequest request)
        {
            if (!int.TryParse(funcionId.ToString(), out funcionId))
            {
                throw new SyntaxErrorException("Formato inválido para el id, pruebe con un entero.");
            }
            if (request.Cantidad < 1)
            {
                throw new BadRequestException("La cantidad no puede ser 0 o negativa.");
            }

            Funciones funcion = await _funcionService.GetFuncionById(funcionId) ?? throw new FuncionNotFoundException("Id de función inexistente.");
            List<TicketItemResponse> ticketItems = new List<TicketItemResponse>();

            if (request.Cantidad + funcion.Tickets.Count <= funcion.Sala.Capacidad)
            {
                for (int i = 0; i < request.Cantidad; i++)
                {
                    Tickets ticket = new Tickets
                    {
                        FuncionId = funcionId,
                        Usuario = request.Usuario
                    };

                    Guid idTicket = await _command.RegisterTicket(ticket);
                    ticketItems.Add(new TicketItemResponse { TicketId = idTicket });
                }

                FuncionGetResponse funcionResponse = new FuncionGetResponse
                {
                    FuncionId = funcion.FuncionId,
                    Pelicula = await _peliculaMapper.GeneratePeliculaGetResponse(funcion.Pelicula),
                    Sala = await _salaMapper.GetSalaResponse(funcion.Sala),
                    Fecha = funcion.Fecha,
                    Horario = funcion.Horario.ToString("hh\\:mm")
                };

                return new TicketResponse
                {
                    Tickets = ticketItems,
                    Funcion = funcionResponse,
                    Usuario = request.Usuario
                };
            }
            else
            {
                throw new ConflictException("No hay suficientes tickets disponibles. Solo quedan: " + (funcion.Sala.Capacidad - funcion.Tickets.Count));
            }
        }
    }
}