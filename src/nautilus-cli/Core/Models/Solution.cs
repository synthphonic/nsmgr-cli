using System.Collections.Generic;
using System.Collections.ObjectModel;
using NautilusCLI.Core.Utils;

namespace NautilusCLI.Core.Models
{
	public class Solution
	{
		private IList<Project> _projects;

		public Solution(string solutionFullPath)
		{
			SolutionFullPath = solutionFullPath;
			SolutionFileName = FileUtil.GetFileName(solutionFullPath);

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

		public string SolutionFileName { get; private set; }
		public string SolutionFullPath { get; private set; }
		public ReadOnlyCollection<Project> Projects { get; private set; }
	}
}