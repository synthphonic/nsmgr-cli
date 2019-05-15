using NautilusCLI.Core.Configurations.Enums;

namespace NautilusCLI.Core.Models
{
	public class ProjectMetadata
	{
		public string ProjectFileName { get; internal set; }
		public string SolutionFileName { get; internal set; }
		public string ParentPath { get; internal set; }
		public string ProjectTypeGuid { get; internal set; }
		public string ProjectName { get; internal set; }
		public string RelativeProjectPath { get; internal set; }
		public string ProjectGuid { get; internal set; }
		public string ProjectFullPath { get; internal set; }
		public SolutionProjectElement ProjectType { get; internal set; }
	}
}
