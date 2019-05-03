using System;

namespace SolutionNugetPackagesUpdater.Core.Exceptions
{
	public class SolutionFileException : ArgumentException
	{
		public SolutionFileException() 
		{
		}

		public SolutionFileException(string message, string paramName) : base(message, paramName)
		{
		}

		public SolutionFileException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
		{
		}

		public SolutionFileException(string message) : base(message)
		{
		}

		public SolutionFileException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}