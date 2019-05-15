/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;
using SolutionNugetPackagesUpdater;

namespace NugetPckgUpdater.CommandLine
{
	[Verb("solutionreport", HelpText = "The solution report to display")]
	class ReportOptions
	{
		[Option("path", HelpText = "The file path to write the result. Should include file name as well", Separator = ':')]
		public string Path { get; set; }
	}

	[Verb("findconflict",  HelpText = "Finds the conflicting nuget package versions installed in the solution")]
	class FindConflict
	{
		[Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file")]
		public string SolutionFileName { get; set; }

		[Option("project", Required = false, Hidden = false, HelpText = "Process project files only and ignore the rest. Default is false")]
		public bool Project { get; set; }

		[Option("debugdata", Required = false, Hidden = true, HelpText = "")]
		public bool DebugData { get; set; }

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