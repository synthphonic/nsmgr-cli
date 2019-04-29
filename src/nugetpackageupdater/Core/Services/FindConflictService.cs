using System;
using System.Collections.Generic;
using System.Linq;
using NugetPckgUpdater.Core;
using NugetPckgUpdater.Core.Configurations;
using SolutionNugetPackagesUpdater.Core.Exceptions;
using SolutionNugetPackagesUpdater.Core.FileReaders;
using SolutionNugetPackagesUpdater.Core.Models;
using SolutionNugetPackagesUpdater.Core.Utils;

namespace SolutionNugetPackagesUpdater.Core.Services
{
    public class FindConflictService
    {
        private string _fileName;
        private IList<SolutionProjectInfo> _spiList;
		private IList<Project> _projects;
		private readonly bool _processProjectsOnly;

		public FindConflictService(string solutionFileName, bool processProjectsOnly = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_fileName = solutionFileName;
			_spiList = new List<SolutionProjectInfo>();
			_projects = new List<Project>();
		}

		public void Run()
		{
			var slnFileReader = new SolutionFileReader();
			var fileContents = slnFileReader.Read(_fileName) as IEnumerable<string>;

			var f = new List<string>(fileContents);
			var searchResults = f.Where(x => x.Contains("Project(")).ToList();

			foreach (var item in searchResults)
			{
				Extract(item);
			}

			FindConflict();

			Output();
		}

		private void FindConflict()
		{
			var parentPath = FileUtil.GetFullPath(_fileName);

			try
			{
				foreach (var item in _spiList)
				{
					var projectType = VisualStudioProjectSetting.GetProjectType(item.ProjectTypeGuid);
					var projectFile = FileUtil.PathCombine(parentPath, item.ProjectPath);

					var fileFinder = new FileReader(projectFile);
					var fileContentObject = fileFinder.ReadFile();
					var packages = fileContentObject as IList<PackageReferenceItemModel>;

					var project = new Project(item.ProjectGuid, item.ProjectName, packages);
					_projects.Add(project);
				}
			}
			catch (PackageManagerReaderException packageManagerEx)
			{
				throw new CLIException(packageManagerEx.Message, packageManagerEx);
			}
			catch (Exception ex)
			{
				throw new CLIException($"Something went wrong in {GetType().Name}", ex);
			}
		}

		private void Extract(string data)
		{
			var spi = SolutionProjectInfo.Extract(data);

			var projectType = VisualStudioProjectSetting.GetProjectType(spi.ProjectTypeGuid);

			if (_processProjectsOnly)
			{
				if (projectType != Configurations.Enums.ProjectType.VirtualFolder)
					_spiList.Add(spi);
			}
			else
			{
				_spiList.Add(spi);
			}
		}

		private void Output()
		{
			var parentPath = FileUtil.GetFullPath(_fileName);

			foreach (var item in _spiList)
			{
				var projectType = VisualStudioProjectSetting.GetProjectType(item.ProjectTypeGuid);
				var projectFile = FileUtil.PathCombine(parentPath, item.ProjectPath);

				var prjTypeMgr = new ProjectTypeManager(projectFile);
				var targetFramework = prjTypeMgr.ProjectType();

				var outputString = targetFramework == Configurations.Enums.TargetFramework.Unknown
					? $"{item.ProjectName} [{projectType.ToString()}] [{targetFramework.ToString()}] [{item.ProjectTypeGuid}]"
					: $"{item.ProjectName} [{projectType.ToString()}] [{targetFramework.ToString()}]";

				Console.WriteLine(outputString);
			}
		}
	}
}