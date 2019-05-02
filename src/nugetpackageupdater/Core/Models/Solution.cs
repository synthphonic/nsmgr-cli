using System.Collections.Generic;
using System.Collections.ObjectModel;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;

namespace SolutionNugetPackagesUpdater.Core.Models
{
	public class Solution
    {
		private IList<Project> _projects;

		public Solution()
		{
			_projects = new List<Project>();
		}

		internal void Done()
		{
			Projects = new ReadOnlyCollection<Project>(_projects);
		}

		internal void AddProject(Project project)
		{
			_projects.Add(project);
		}

		public string ParentPath { get; internal set; }
		public string SolutionFileName { get; internal set; }
		public string ProjectName { get; internal set; }
		public string ProjectFileName { get; internal set; }
		public string ProjectTypeGuid { get; internal set; }
        public string RelativeProjectPath { get; internal set; }
        public string ProjectGuid { get; internal set; }
		public ReadOnlyCollection<Project> Projects { get; internal set; }
	}
}
