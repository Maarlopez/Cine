using Application.Response;

namespace Application.Response
{
    public class PeliculaGetResponse
    {
        public int PeliculaId { get; set; }
        public string Titulo { get; set; }
        public string Poster { get; set; }
        public Genero Genero { get; set; }
    }
}
