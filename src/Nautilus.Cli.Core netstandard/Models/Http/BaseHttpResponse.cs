using System;

namespace Nautilus.Cli.Core.Models.Http
{
	public class BaseHttpResponse
	{
		public BaseHttpResponse()
		{

		}

		public BaseHttpResponse(Exception ex)
		{
			Exception = ex;
		}

		public Exception Exception { get; private set; }
	}
}