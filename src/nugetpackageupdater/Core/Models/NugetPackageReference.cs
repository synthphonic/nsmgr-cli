using NautilusCLI.Core.Configurations.Enums;

namespace NautilusCLI.Core.Models
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

	public class NugetPackageReferenceExtended : NugetPackageReference
	{
		public NugetPackageReferenceExtended(Project project, NugetPackageReference nugetPackage)
			: base(nugetPackage.PackageName, nugetPackage.Version, nugetPackage.TargetFramework)
		{
			ProjectName = project.ProjectName;
			ProjectTargetFramework = project.TargetFramework;
		}

		public NugetPackageReferenceExtended(string projectName, NugetPackageReference nugetPackage)
			: base(nugetPackage.PackageName, nugetPackage.Version, nugetPackage.TargetFramework)
		{
			ProjectName = projectName;
		}

		public string ProjectName { get; private set; }

		public ProjectTarget ProjectTargetFramework { get; private set; }

		public string PackageVersionName
		{
			get
			{
				return $"{PackageName}-{Version}";
			}
		}
	}
}