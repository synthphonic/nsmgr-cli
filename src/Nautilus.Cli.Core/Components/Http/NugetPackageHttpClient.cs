namespace Nautilus.Cli.Core.Components.Http
{
	public class NugetPackageHttpClient
	{
		public static NugetPackageQuery QueryRequest(string packageName, bool preRelease)
		{
			return new NugetPackageQuery(packageName, preRelease);
		}
	}
}