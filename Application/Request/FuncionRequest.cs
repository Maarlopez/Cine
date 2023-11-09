namespace Application.Request
{
    public class FuncionRequest
    {
        public int Pelicula { get; set; }
        public int Sala { get; set; }
        public string Fecha { get; set; }
        public string Horario { get; set; } = null!;
    }
}