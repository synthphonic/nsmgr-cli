using System.Collections.Generic;
using NugetPckgUpdater.Core.Models;

namespace NugetPckgUpdater.Core.Configurations
{
    public static class NETStandardVersion
    {
        private static Dictionary<string, ProjectType> _versions;

		static NETStandardVersion()
		{
			_versions = new Dictionary<string, ProjectType>();
			_versions["netstandard2.0"] = ProjectType.NETStandard20;
			_versions["netcoreapp2.2"] = ProjectType.NETCoreApp22;
			_versions["netcoreapp2.1"] = ProjectType.NETCoreApp21;
			_versions["netcoreapp2.0"] = ProjectType.NETCoreApp20;
		}

        internal static ProjectType Get(string innerText)
        {
            return _versions[innerText];
        }
    }
}
