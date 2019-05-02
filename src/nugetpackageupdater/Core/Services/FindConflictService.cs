using System;
using System.Collections.Generic;
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
        private string _fileName;
        private IList<ProjectMetadata> _spiList;
		private IList<Project> _projects;
		private readonly bool _processProjectsOnly;

		public FindConflictService(string solutionFileName, bool processProjectsOnly = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_fileName = solutionFileName;
			_spiList = new List<ProjectMetadata>();
			_projects = new List<Project>();
		}

		public void Run()
		{
			var slnFileReader = new SolutionFileReader(_fileName, _processProjectsOnly);
			var solution = slnFileReader.Read();

			//FindConflict();

			//Output();
		}

		private void Output()
		{
			var parentPath = FileUtil.GetFullPath(_fileName);

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