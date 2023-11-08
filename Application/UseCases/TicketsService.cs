using Application.Exceptions;
using Application.IMappers;
using Application.Interfaces;
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
        private readonly ITicketMapper _ticketMapper;

        public TicketsService(ITicketCommand ticketCommand, ITicketQuery ticketQuery, IFuncionesService FuncionService, ITicketMapper ticketMapper)
        {
            _command = ticketCommand;
            _query = ticketQuery;
            _funcionService = FuncionService;
            _ticketMapper = ticketMapper;
        }
        public async Task<TicketResponse> createTicket(int funcionId, TicketsRequest request)
        {
            try
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
                List<Guid> ticketsIds = new();

                if (request.Cantidad + funcion.Tickets.Count <= funcion.Sala.Capacidad)
                {
                    for (int i = 0; i < request.Cantidad; i++)
                    {
                        Tickets ticket = new()
                        {
                            FuncionId = funcionId,
                            Usuario = request.Usuario,
                        };

                        Guid idTicket = await _command.RegisterTicket(ticket);
                        ticketsIds.Add(idTicket);
                    }

                    return await _ticketMapper.GenerateTicketResponse(await _query.GetTicketById(ticketsIds[0]), ticketsIds);
                }
                else
                {
                    throw new ConflictException("No hay suficientes tickets disponibles. Solo quedan: " + (funcion.Sala.Capacidad - funcion.Tickets.Count));
                }
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException("Error en la creación del ticket: " + ex.Message);
            }
            catch (SyntaxErrorException ex)
            {
                throw new SyntaxErrorException("Error en la sintaxis: " + ex.Message);
            }
            catch (BadRequestException ex)
            {
                throw new BadRequestException("Error en el pedido: " + ex);
            }
            catch (ConflictException ex)
            {
                throw new ConflictException("Error en el pedido: " + ex);
            }
        }
    }
}