/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;
using Nautilus.Cli.Core.Abstraction;

namespace Nautilus.Cli.Client.CommandLine.Verbs
{
	[Verb("list-packages",  HelpText = "List nuget packages for all projects in the solution")]
	class ListNugetPackages : VerbBase
	{
		private const string Example_Text = "Finds any conflicting nuget package versions in the solution";

		[Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file")]
		public string SolutionFileName { get; set; }

		[Option("projects-only", Default = true,  Required = false, Hidden = true, HelpText = "Process project files only and ignore the rest. Default is false")]
		public bool ProjectsOnly { get; set; }

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
					new Example(Example_Text, new ListNugetPackages{ SolutionFileName=platformPathSample} )
				};
			}
		}
	}
}