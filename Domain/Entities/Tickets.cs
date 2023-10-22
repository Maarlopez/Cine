namespace Domain.Entities
{
    public class Tickets
    {
        public Guid TicketId { get; set; }
        public int FuncionId { get; set; }
        public string Usuario { get; set; }

        // Relaciones de navegación
        public Funciones Funcion { get; set; }
    }
}
