using System.Diagnostics;

namespace SolutionNugetPackagesUpdater.Core.Models
{
	public class SolutionProjectInfo
    {
        internal static SolutionProjectInfo Extract(string data)
		{
			Debug.WriteLine(data);

			var split = data.Split(',');

			var projectPath = GetProjectPath(split);
			var projectGuid = GetProjectGuid(split);
			var projectTypeGuid = GetProjectTypeGuid(split[0]);
			var projectName = GetProjectName(split[0], projectTypeGuid);

			var spi = new SolutionProjectInfo
			{
				ProjectTypeGuid = projectTypeGuid,
				ProjectName = projectName,
				ProjectPath = projectPath,
				ProjectGuid = projectGuid
			};

			return spi;
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

		public string ProjectName { get; internal set; }
        public string ProjectTypeGuid { get; internal set; }
        public string ProjectPath { get; internal set; }
        public string ProjectGuid { get; internal set; }
    }
}
