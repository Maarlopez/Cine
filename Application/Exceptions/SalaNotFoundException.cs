namespace Application.Exceptions
{
    public class SalaNotFoundException : Exception
    {
        public SalaNotFoundException()
        {
        }

        public SalaNotFoundException(string message)
            : base(message)
        {
        }

        public SalaNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}