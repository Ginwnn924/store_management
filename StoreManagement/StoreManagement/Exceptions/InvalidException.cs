namespace StoreManagement.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException()
        {
        }
        public InvalidException(string message)
            : base(message)
        {
        }
        public InvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
