namespace Domain.Entities
{
    public class Salas
    {
        public int SalaId { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }

        // Relaciones de navegación
        public ICollection<Funciones> Funciones { get; set; }
    }
}
