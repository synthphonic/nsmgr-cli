using System;

namespace Nautilus.Cli.Core.Exceptions
{
	public class NugetPackageNotFoundException : Exception
	{
		public NugetPackageNotFoundException()
		{
		}

		public NugetPackageNotFoundException(string message) : base(message)
		{
		}

		public NugetPackageNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}