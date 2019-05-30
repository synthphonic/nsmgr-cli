using System.Collections.Generic;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Helpers;
using Nautilus.Cli.Core.TestData;

namespace Nautilus.Cli.Core.Models
{
	public class Project
	{
		public Project(ProjectMetadata metadata)
		{
			Metadata = metadata;

			if (TestDataHelper.UseTestData)
			{
				Metadata.ProjectName = Metadata.ProjectName.Replace("Storiveo", "FourtyNineLabs");
				Metadata.ProjectName = Metadata.ProjectName.Replace("Niu", "FourtyNineLabs");
			}

			if (Metadata.ProjectType == SolutionProjectElement.CSharpProject)
			{
				var projectTypeManager = new ProjectTypeManager(Metadata.ProjectFullPath);
				TargetFramework = projectTypeManager.GetTargetFramework();
				ProjectType = Metadata.ProjectType;

				var fileFinder = new FileReader(TargetFramework, Metadata);
				var fileContentObject = fileFinder.ReadFile();
				Packages = fileContentObject as IEnumerable<NugetPackageReference>;
			}
			else if (Metadata.ProjectType == SolutionProjectElement.VirtualFolder)
			{
				TargetFramework = ProjectTarget.Unknown;
			}

			ProjectType = Metadata.ProjectType;
		}

		public string ProjectGuid { get { return Metadata.ProjectGuid; } }
		public string ProjectName { get { return Metadata.ProjectName; } }
		public ProjectTarget TargetFramework { get; private set; }
		public IEnumerable<NugetPackageReference> Packages { get; private set; }
		public SolutionProjectElement ProjectType { get; private set; }
		public ProjectMetadata Metadata { get; private set; }
	}
}