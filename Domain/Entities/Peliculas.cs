namespace Domain.Entities
{
    public class Peliculas
    {
        public int PeliculaId { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }
        public int GeneroId { get; set; }

        // Relaciones de navegación
        public Generos Genero { get; set; }
        public ICollection<Funciones> Funciones { get; set; }
    }
}
