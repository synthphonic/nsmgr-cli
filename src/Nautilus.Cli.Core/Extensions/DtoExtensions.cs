using System.Linq;
using Nautilus.Cli.Core.Models.Http;

namespace Nautilus.Cli.Core.Extensions
{
	public static class DtoExtensions
	{
		public static string GetCurrentVersion(this QueryPackageResponse value, string packageName)
		{
			var found = value.Data.FirstOrDefault(x => x.Title.Equals(packageName));
			if (found != null)
			{
				return found.Version;
			}
			return string.Empty;
		}
	}
}