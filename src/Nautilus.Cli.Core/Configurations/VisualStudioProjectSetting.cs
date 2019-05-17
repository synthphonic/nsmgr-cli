using System.Collections.Generic;
using System.Linq;
using Nautilus.Cli.Core.Configurations.Enums;

namespace NugetPckgUpdater.Core.Configurations
{
	/// <summary>
	/// Refer to the following links for more info 
	/// </summary>
	/// <remarks>
	/// <see cref="https://www.google.com/search?q=FAE04EC0-301F-11D3-BF4B-00C04F79EFBC"/>
	/// <para />
	/// <see cref="https://stackoverflow.com/questions/10802198/visual-studio-project-type-guids"/>
	/// </remarks>
	public static class VisualStudioProjectSetting
    {
		private static Dictionary<SolutionProjectElement, string> _projectTypeGuidList;

        public const string CSharpTypeGuid = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
        public const string AndroidTypeGuid = "EFBA0AD7-5A72-4C68-AF49-83D382785DCF";
        public const string iOSTypeGuid = "FEACFBD2-3405-455C-9665-78FE426C6842";
		public const string VirtualFolderGuid = "2150E333-8FDC-42A3-9474-1A3956D46DE8";
		public const string iOSBindingTypeGuid = "8FFB629D-F513-41CE-95D2-7ECE97B6EEEC";

		static VisualStudioProjectSetting()
		{
			_projectTypeGuidList = new Dictionary<SolutionProjectElement, string>
			{
				{ SolutionProjectElement.CSharpProject, CSharpTypeGuid },
				{ SolutionProjectElement.VirtualFolder, VirtualFolderGuid }
			};
		}

		internal static SolutionProjectElement GetProjectType(string projectTypeGuid)
		{
			if (!_projectTypeGuidList.ContainsValue(projectTypeGuid))
			{
				return SolutionProjectElement.Unknown;
			}

			var result = _projectTypeGuidList.FirstOrDefault(x => x.Value.Equals(projectTypeGuid));
			return result.Key;
		}
	}
}