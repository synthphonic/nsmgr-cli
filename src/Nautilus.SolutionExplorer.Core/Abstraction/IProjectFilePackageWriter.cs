using Nautilus.SolutionExplorer.Core.Configurations.Enums;
using Nautilus.SolutionExplorer.Core.Models;

namespace Nautilus.SolutionExplorer.Core.Abstraction
{
	public interface IProjectFilePackageWriter
	{
		void Initialize(ProjectTargetFramework targetFramework, ProjectMetadata projectMetadata);
		void UpdatePackageReference(string packageName, string newVersion);
	}
}
