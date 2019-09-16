using System;

namespace Nautilus.Cli.Core.Extensions
{
	public static class ExceptionExtension
	{
		public static Exception GetFirstException(this Exception ex)
		{
			if (ex.InnerException == null)
			{
				return ex;
			}

			return GetFirstException(ex.InnerException);
		}
	}
}