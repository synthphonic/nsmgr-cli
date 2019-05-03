using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NugetPckgUpdater.Core.Configurations;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;
using SolutionNugetPackagesUpdater.Core.FileReaders;
using SolutionNugetPackagesUpdater.Core.Helpers;
using SolutionNugetPackagesUpdater.Core.Models;
using SolutionNugetPackagesUpdater.Core.Utils;

namespace SolutionNugetPackagesUpdater.Core.Services
{
	public class FindConflictService
    {
        private string _solutionFileName;
        private IList<ProjectMetadata> _spiList;
		private readonly bool _processProjectsOnly;

		public FindConflictService(string solutionFileName, bool processProjectsOnly = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_solutionFileName = solutionFileName;
			_spiList = new List<ProjectMetadata>();
		}

		public void Run()
		{
			var slnFileReader = new SolutionFileReader(_solutionFileName, _processProjectsOnly);
			var solution = slnFileReader.Read();

			var projectCounter = 1;
			foreach (var project in solution.Projects)
			{
				Colorful.Console.WriteLine($"{projectCounter}. {project.ProjectName}", Color.YellowGreen);
				Colorful.Console.WriteLine($"Found {project.Packages.Count()} nuget packages", Color.YellowGreen);
				Colorful.Console.WriteLine();

				projectCounter++;
			}

			//FindConflict();

			//Output();
		}

		private void Output()
		{
			var parentPath = FileUtil.GetFullPath(_solutionFileName);

			foreach (var item in _spiList)
			{
				var projectType = VisualStudioProjectSetting.GetProjectType(item.ProjectTypeGuid);
				var projectFile = FileUtil.PathCombine(parentPath, item.RelativeProjectPath);

				var prjTypeMgr = new ProjectTypeManager(projectFile);
				var targetFramework = prjTypeMgr.GetTargetFramework();

				var outputString = targetFramework == ProjectTarget.Unknown
					? $"{item.ProjectName} [{projectType.ToString()}] [{targetFramework.ToString()}] [{item.ProjectTypeGuid}]"
					: $"{item.ProjectName} [{projectType.ToString()}] [{targetFramework.ToString()}]";

				Console.WriteLine(outputString);
			}
		}
	}
}