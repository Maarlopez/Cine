namespace Application.Exceptions
{
    public class GeneroNotFoundException : Exception
    {
        public GeneroNotFoundException()
        {
        }

        public GeneroNotFoundException(string message)
            : base(message)
        {
        }

        public GeneroNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}