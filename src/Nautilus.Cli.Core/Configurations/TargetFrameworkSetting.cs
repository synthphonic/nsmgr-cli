using System;
using System.Collections.Generic;
using System.Linq;
using Nautilus.Cli.Core.Configurations.Enums;

namespace Nautilus.Cli.Core.Configurations
{
	public static class TargetFrameworkSetting
    {
        private static Dictionary<string, ProjectTarget> _versions;

		static TargetFrameworkSetting()
		{
			_versions = new Dictionary<string, ProjectTarget>
			{
				["netstandard2.0"] = ProjectTarget.NETStandard20,
				["netcoreapp2.2"] = ProjectTarget.NETCoreApp22,
				["netcoreapp2.1"] = ProjectTarget.NETCoreApp21,
				["netcoreapp2.0"] = ProjectTarget.NETCoreApp20
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
		internal static ProjectTarget Get(string stringValue)
        {
			try
			{
				var found = _versions.FirstOrDefault(x => x.Key.Equals(stringValue));
			}
			catch (ArgumentNullException)
			{
				return ProjectTarget.Unknown;
			}


            return _versions[stringValue];
        }
    }
}
