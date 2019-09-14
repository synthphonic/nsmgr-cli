using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core.FileWriters
{
	public class PackageConfigFileWriter : IProjectFilePackageWriter
	{
		#region IProjectFilePackageWriter implementation
		public void Initialize(ProjectTarget targetFramework, ProjectMetadata projectMetadata)
		{
			// SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
		}

		public void UpdatePackageReference(string packageName, string newVersion)
		{
			// SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
		}
		#endregion
	}
}