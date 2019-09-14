using System.Collections.Generic;
using System.Linq;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core.Configurations
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
        private static IList<ProjectTypeInfo> _projectTypeGuidList;

        public const string CSharpTypeGuid = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
        public const string CSharpTypeGuid2 = "9A19103F-16F7-4668-BE54-9A1E7A4F7556"; // refer to https://github.com/xamarin/mirepoix/issues/2
        public const string AndroidTypeGuid = "EFBA0AD7-5A72-4C68-AF49-83D382785DCF";
        public const string iOSTypeGuid = "FEACFBD2-3405-455C-9665-78FE426C6842";
        public const string UWPTypeGuid = "A5A43C5B-DE2A-4C0C-9213-0A381AF9435A";
		public const string VirtualFolderGuid = "2150E333-8FDC-42A3-9474-1A3956D46DE8";
		public const string iOSBindingTypeGuid = "8FFB629D-F513-41CE-95D2-7ECE97B6EEEC";
        
        static VisualStudioProjectSetting()
		{
            _projectTypeGuidList = new List<ProjectTypeInfo>();
            _projectTypeGuidList.Add(new ProjectTypeInfo(SolutionProjectElement.CSharpProject, CSharpTypeGuid));
            _projectTypeGuidList.Add(new ProjectTypeInfo(SolutionProjectElement.CSharpProject, CSharpTypeGuid2));
            _projectTypeGuidList.Add(new ProjectTypeInfo(SolutionProjectElement.VirtualFolder, VirtualFolderGuid));
		}

		internal static SolutionProjectElement GetProjectType(string projectTypeGuid)
		{
            var found = _projectTypeGuidList.FirstOrDefault(x => x.Guid.Equals(projectTypeGuid));
            if (found == null)
            {
                return SolutionProjectElement.Unknown;
            }

            return found.SolutionProjectElement;
		}
	}
}