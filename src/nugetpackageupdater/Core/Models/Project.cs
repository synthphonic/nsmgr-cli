using System.Collections.Generic;

namespace SolutionNugetPackagesUpdater.Core.Models
{
	public class Project
	{
		public Project(string projectGuid, string projectName, IEnumerable<PackageReferenceItemModel> packages)
		{
			ProjectGuid = projectGuid;
			ProjectName = projectName;
			Packages = packages;
		}

		public string ProjectGuid { get; private set; }
		public string ProjectName { get; private set; }

		public IEnumerable<PackageReferenceItemModel> Packages { get; private set; }
	}
}