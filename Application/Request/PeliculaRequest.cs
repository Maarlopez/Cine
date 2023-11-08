namespace Application.Request
{
    public class PeliculaRequest
    {
        public string Titulo { get; set; } = null!;
        public string Poster { get; set; } = null!;
        public string Trailer { get; set; } = null!;
        public string Sinopsis { get; set; } = null!;
        public int Genero { get; set; }
    }
}