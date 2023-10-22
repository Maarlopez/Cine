namespace Domain.Entities
{
    public class Generos
    {
        public int GeneroId { get; set; }
        public string Nombre { get; set; }

        // Relacion de navegacion
        public ICollection<Peliculas> Peliculas { get; set; }
    }
}