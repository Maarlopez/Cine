namespace Application.Response
{
    public class TicketResponse
    {
        public List<TicketItemResponse> Tickets { get; set; } = new List<TicketItemResponse>();
        public FuncionGetResponse Funcion { get; set; }
        public string? Usuario { get; set; }
    }
}
