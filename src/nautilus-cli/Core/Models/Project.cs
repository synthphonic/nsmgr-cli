using System.Collections.Generic;
using NautilusCLI.Core.Configurations.Enums;
using NautilusCLI.Core.Helpers;
using NautilusCLI.TestData;

namespace NautilusCLI.Core.Models
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

			var projectTypeManager = new ProjectTypeManager(_metadata.ProjectFullPath);
			TargetFramework = projectTypeManager.GetTargetFramework();

			var fileFinder = new FileReader(TargetFramework, _metadata);
			var fileContentObject = fileFinder.ReadFile();
			Packages = fileContentObject as IEnumerable<NugetPackageReference>;
		}

		public string ProjectGuid { get { return _metadata.ProjectGuid; } }
		public string ProjectName { get { return _metadata.ProjectName; } }
		public ProjectTarget TargetFramework { get; private set; }
		public IEnumerable<NugetPackageReference> Packages { get; private set; }
	}
}