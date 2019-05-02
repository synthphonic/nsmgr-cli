namespace SolutionNugetPackagesUpdater.Core.Models
{
	public class NugetPackageReference
	{
		public NugetPackageReference(string packageName, string version, string targetFramework = "")
		{
			PackageName = packageName;
			Version = version;
			TargetFramework = targetFramework;
		}

		public string PackageName { get; private set; }
		public string Version { get; private set; }
		public string TargetFramework { get; private set; }

	}
}