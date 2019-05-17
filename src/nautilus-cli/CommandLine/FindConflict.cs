/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;

namespace Nautilus.Cli.Client.CommandLine
{
	[Verb("findconflict",  HelpText = "Finds the conflicting nuget package versions installed in the solution")]
	class FindConflict
	{
		[Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file")]
		public string SolutionFileName { get; set; }

		[Option("project", Default = true,  Required = false, Hidden = true, HelpText = "Process project files only and ignore the rest. Default is false")]
		public bool Project { get; set; }

		[Option("usedebugdata", Required = false, Hidden = true, HelpText = "")]
		public bool UseDebugData { get; set; }

		[Usage(ApplicationAlias = Program.Name)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				var platformPathSample = string.Empty;
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					platformPathSample = "/users/itsme/xxx.sln";
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					platformPathSample = @"C:\myproject\xxx.sln";
				}

				return new List<Example>
				{
					new Example("Finds any conflicting nuget package versions in the solution",new FindConflict{ SolutionFileName=platformPathSample} )
				};
			}
		}
	}
}