using System.Diagnostics;
using System.IO;
using NugetPckgUpdater.Core.Configurations;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.Utils;

namespace Nautilus.Cli.Core.Helpers
{
	public static class ProjectMetadataExtractor
	{
		internal static ProjectMetadata Extract(string data, string solutionFileName)
		{
			Debug.WriteLine(data);

			var split = data.Split(',');
			var relativeProjectPath = GetProjectPath(split);
			var projectGuid = GetProjectGuid(split);
			var projectTypeGuid = GetProjectTypeGuid(split[0]);
			var projectName = GetProjectName(split[0], projectTypeGuid);
			var projectFileName = FileUtil.GetFileName(relativeProjectPath);
			var parentPath = FileUtil.GetFullPath(solutionFileName);
			var projectFullPath = Path.Combine(parentPath, relativeProjectPath);

			var metadata = new ProjectMetadata
			{
				ProjectFileName = projectFileName,
				SolutionFileName = solutionFileName,
				ParentPath = FileUtil.GetFullPath(solutionFileName),
				ProjectTypeGuid = projectTypeGuid,
				ProjectName = projectName,
				RelativeProjectPath = relativeProjectPath,
				ProjectGuid = projectGuid,
				ProjectFullPath = projectFullPath,
				ProjectType = VisualStudioProjectSetting.GetProjectType(projectTypeGuid)
			};

			return metadata;
		}

		private static string GetProjectGuid(string[] split)
		{
			var projectGuid = split[2].Trim().Replace("{", string.Empty).Replace("}", string.Empty);
			projectGuid = projectGuid.Replace("\"", string.Empty);

			return projectGuid;
		}

		private static string GetProjectPath(string[] split)
		{
			var projectPath = split[1].Trim();
			projectPath = projectPath.Replace("\"", string.Empty);
			projectPath = projectPath.Replace("\\", "/");

			return projectPath;
		}

		private static string GetProjectTypeGuid(string zeroIndexString)
		{
			var cleanedString = ExtractAndCleanStringAtZeroIndex(zeroIndexString);
			var projectTypeGuid = cleanedString.Substring(0, 36);

			return projectTypeGuid;
		}

		private static string GetProjectName(string zeroIndexString, string projectTypeGuid)
		{
			var cleanedString = ExtractAndCleanStringAtZeroIndex(zeroIndexString);
			var projectName = cleanedString.Replace($"{projectTypeGuid} \"", string.Empty);

			return projectName;
		}

		private static string ExtractAndCleanStringAtZeroIndex(string zeroIndexString)
		{
			var split = zeroIndexString.Split(',');
			var extractedString = split[0].Replace("Project(\"{", string.Empty);
			extractedString = extractedString.Replace("}\") =", string.Empty);
			extractedString = extractedString.Substring(0, extractedString.Length - 1);

			return extractedString;
		}

	}
}