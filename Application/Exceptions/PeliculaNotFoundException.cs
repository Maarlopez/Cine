namespace Application.Exceptions
{
    public class PeliculaNotFoundException : Exception
    {
        public PeliculaNotFoundException()
        {
        }

        public PeliculaNotFoundException(string message)
            : base(message)
        {
        }

        public PeliculaNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}