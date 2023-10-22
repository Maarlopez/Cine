namespace Domain.Entities
{
    public class Funciones
    {
        public int FuncionId { get; set; }
        public int PeliculaId { get; set; }
        public int SalaId { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Horario { get; set; }


        // Relaciones de navegación
        public Peliculas Pelicula { get; set; }
        public Salas Sala { get; set; }
        public ICollection<Tickets> Tickets { get; set; }
    }
}
