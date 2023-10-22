namespace Application.Exceptions
{
    public class FuncionNotFoundException : Exception
    {
        public FuncionNotFoundException()
        {
        }

        public FuncionNotFoundException(string message)
            : base(message)
        {
        }

        public FuncionNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}