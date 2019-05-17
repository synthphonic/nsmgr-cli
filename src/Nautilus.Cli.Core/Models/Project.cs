using System.Collections.Generic;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Helpers;
using Nautilus.Cli.Core.TestData;

namespace Nautilus.Cli.Core.Models
{
	public class Project
	{
		private ProjectMetadata _metadata;

		public Project(ProjectMetadata metadata)
		{
			_metadata = metadata;

			if (TestDataHelper.UseTestData)
			{
				_metadata.ProjectName = _metadata.ProjectName.Replace("Storiveo", "FourtyNineLabs");
				_metadata.ProjectName = _metadata.ProjectName.Replace("Niu", "FourtyNineLabs");
			}

			if (_metadata.ProjectType == SolutionProjectElement.CSharpProject)
			{
				var projectTypeManager = new ProjectTypeManager(_metadata.ProjectFullPath);
				TargetFramework = projectTypeManager.GetTargetFramework();
				ProjectType = _metadata.ProjectType;

				var fileFinder = new FileReader(TargetFramework, _metadata);
				var fileContentObject = fileFinder.ReadFile();
				Packages = fileContentObject as IEnumerable<NugetPackageReference>;
			}
			else if (_metadata.ProjectType == SolutionProjectElement.VirtualFolder)
			{
				TargetFramework = ProjectTarget.Unknown;
			}

			ProjectType = _metadata.ProjectType;
		}

		public string ProjectGuid { get { return _metadata.ProjectGuid; } }
		public string ProjectName { get { return _metadata.ProjectName; } }
		public ProjectTarget TargetFramework { get; private set; }
		public IEnumerable<NugetPackageReference> Packages { get; private set; }
		public SolutionProjectElement ProjectType { get; private set; }
	}
}