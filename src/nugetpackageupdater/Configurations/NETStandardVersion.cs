using System.Collections.Generic;
using NugetPckgUpdater.Models;

namespace NugetPckgUpdater.Configurations
{
    public static class NETStandardVersion
    {
        private static Dictionary<string, ProjectType> _versions;

        static NETStandardVersion()
        {
            _versions = new Dictionary<string, ProjectType>();
            _versions["netstandard2.0"] = ProjectType.NETStandard20;
            _versions["netcoreapp2.1"] = ProjectType.NETCoreApp21;
            _versions["netcoreapp2.0"] = ProjectType.NETCoreApp20;
        }

        internal static ProjectType Get(string innerText)
        {
            return _versions[innerText];
        }
    }
}
