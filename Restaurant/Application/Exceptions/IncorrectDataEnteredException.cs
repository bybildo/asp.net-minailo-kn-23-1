namespace Restaurant.Application.Exceptions
{
    public class IncorrectDataEnteredException : Exception
    {
        public IncorrectDataEnteredException(string message) : base(message) { }
    }
}
