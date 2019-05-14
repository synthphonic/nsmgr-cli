using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;
using SolutionNugetPackagesUpdater.Core.Exceptions;
using SolutionNugetPackagesUpdater.Core.Helpers;
using SolutionNugetPackagesUpdater.Core.Models;
using SolutionNugetPackagesUpdater.Core.Utils;

namespace SolutionNugetPackagesUpdater.Core.FileReaders
{
	public class SolutionFileReader
    {
		private readonly bool _processProjectsOnly; 
		private string _solutionFileName;
		private IList<ProjectMetadata> _projectMetadataList;

		public SolutionFileReader(string solutionFileName, bool processProjectsOnly = false)
		{
			_solutionFileName = solutionFileName;
			_processProjectsOnly = processProjectsOnly;
			_projectMetadataList = new List<ProjectMetadata>();
		}

		public Solution Read()
		{
			if (!File.Exists(_solutionFileName))
			{
				throw new SolutionFileException($"Solution file '{_solutionFileName}' is invalid", "solutionFileName");
			}

			var solutionFileContents = ReadSolutionFile();

			var f = new List<string>(solutionFileContents);
			var searchResults = f.Where(x => x.Contains("Project(")).ToList();

			var solution = new Solution(_solutionFileName);

			foreach (var item in searchResults)
			{
				var parentPath = FileUtil.GetFullPath(_solutionFileName);
				ExtractProjectMetadata(item, _solutionFileName);
			}

			foreach (var metadata in _projectMetadataList)
			{
				var project = new Project(metadata);
				solution.AddProject(project);
			}

			solution.Done();

			return solution;
		}

		private void ExtractProjectMetadata(string data, string fileName)
		{
			var projectMetadata = ProjectMetadataExtractor.Extract(data, fileName);

			if (_processProjectsOnly)
			{
				if (projectMetadata.ProjectType != SolutionProjectElement.VirtualFolder)
					_projectMetadataList.Add(projectMetadata);
			}
			else
			{
				_projectMetadataList.Add(projectMetadata);
			}
		}

		private IEnumerable<string> ReadSolutionFile()
        {
            var fileContents = new List<string>();

            using (var sr = new StreamReader(_solutionFileName,Encoding.UTF8))
            {
                while (string.IsNullOrEmpty(sr.ReadLine()))
                {
                }

                var line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    fileContents.Add(line);
                    line = sr.ReadLine();
                }

                sr.Close();
            }

            return fileContents;
        }
    }
}