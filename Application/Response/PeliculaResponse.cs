namespace Application.Response
{
    public class PeliculaResponse
    {
        public int PeliculaId { get; set; }
        public string Titulo { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }
        public string Sinopsis { get; set; }
        public Genero Genero { get; set; }
        public List<FuncionDelete> Funciones { get; set; } = new List<FuncionDelete>();
    }
}
