using System;
using System.Collections.Generic;
using System.Linq;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;

namespace NugetPckgUpdater.Core.Configurations
{
	public static class TargetFrameworkSetting
    {
        private static Dictionary<string, TargetFramework> _versions;

		static TargetFrameworkSetting()
		{
			_versions = new Dictionary<string, TargetFramework>
			{
				["netstandard2.0"] = TargetFramework.NETStandard20,
				["netcoreapp2.2"] = TargetFramework.NETCoreApp22,
				["netcoreapp2.1"] = TargetFramework.NETCoreApp21,
				["netcoreapp2.0"] = TargetFramework.NETCoreApp20
			};
		}

		/// <summary>
		/// Get the specified innerText.
		/// <para/>
		/// Sample string is as follows
		/// <para/>
		/// netstandard2.0, netcoreapp2.2, netcoreapp2.1, netcoreapp2.0
		/// </summary>
		/// <returns>The get.</returns>
		/// <param name="stringValue">Inner text.</param>
		internal static TargetFramework Get(string stringValue)
        {
			try
			{
				var found = _versions.FirstOrDefault(x => x.Key.Equals(stringValue));
			}
			catch (ArgumentNullException)
			{
				return TargetFramework.Unknown;
			}


            return _versions[stringValue];
        }
    }
}
