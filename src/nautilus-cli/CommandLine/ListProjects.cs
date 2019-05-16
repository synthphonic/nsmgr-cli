/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;

namespace NautilusCLI.CommandLine
{
	[Verb("list-projects", HelpText = "The solution report to display")]
	class ListProjects
	{
		[Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file.")]
		public string SolutionFileName { get; set; }

		[Option("projects-only", HelpText = "Process project files only and ignore the rest.")]
		public bool ProjectsOnly { get; set; }

		[Option("show-nuget-packages", Default = false, Required = false, Hidden = false, HelpText = "Display nuget packages for each project.")]
		public bool ShowNugetPackages { get; set; }

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
					new Example("List out all projects under the solution.",new ListProjects{ SolutionFileName=platformPathSample} )
				};
			}
		}
	}
}
