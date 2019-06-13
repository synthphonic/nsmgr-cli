using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core.Abstraction
{
	public interface IProjectFilePackageWriter
	{
		void Initialize(ProjectTarget targetFramework, ProjectMetadata projectMetadata);
		void UpdatePackageReference(string packageName, string newVersion);
	}
}
