namespace Nautilus.SolutionExplorer.Core.Exceptions;

public class PackageManagerReaderException : Exception
{
    public PackageManagerReaderException()
    {
    }

    public PackageManagerReaderException(string message) : base(message)
    {
    }

    public PackageManagerReaderException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
