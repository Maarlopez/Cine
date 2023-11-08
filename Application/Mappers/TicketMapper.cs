using Application.IMappers;
using Application.Response;
using Domain.Entities;

namespace Application.Mappers
{
    public class TicketMapper : ITicketMapper
    {
        private readonly IPeliculaMapper _peliculaMapper;
        private readonly ISalaMapper _salaMapper;

        public TicketMapper(IPeliculaMapper peliculaMapper, ISalaMapper salaMapper)
        {
            _peliculaMapper = peliculaMapper;
            _salaMapper = salaMapper;
        }

        public async Task<TicketResponse> GenerateTicketResponse(Tickets ticket, List<Guid> listaIds)
        {
            return new TicketResponse
            {
                //Tickets = listaIds,
                Funcion = new FuncionGetResponse
                {
                    FuncionId = ticket.Funcion.FuncionId,
                    Pelicula = await _peliculaMapper.GeneratePeliculaGetResponse(ticket.Funcion.Pelicula),
                    Sala = await _salaMapper.GetSalaResponse(ticket.Funcion.Sala),
                    Fecha = ticket.Funcion.Fecha,
                    Horario = ticket.Funcion.Horario.ToString("hh\\:mm")
                },
                Usuario = ticket.Usuario,
            };
        }

        public async Task<CantidadTicketsResponse> GetCantidadTicketResponse(int capacidad, int ticketCount)
        {
            return await Task.FromResult(new CantidadTicketsResponse
            {
                Cantidad = capacidad - ticketCount,
            });
        }
    }
}
