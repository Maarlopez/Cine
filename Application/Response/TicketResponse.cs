namespace Application.Response
{
    public class TicketResponse
    {
        public List<TicketItemReponse> Tickets { get; set; } = new List<TicketItemReponse>();
        public FuncionGetResponse Funcion { get; set; }
        public string? Usuario { get; set; }
    }
}
