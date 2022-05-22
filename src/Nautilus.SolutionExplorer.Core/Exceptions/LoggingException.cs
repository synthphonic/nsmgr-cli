namespace Nautilus.SolutionExplorer.Core.Exceptions;

public class LoggingException : NullReferenceException
{
    public LoggingException()
    {
    }

    public LoggingException(string message) : base(message)
    {
    }

    public LoggingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
