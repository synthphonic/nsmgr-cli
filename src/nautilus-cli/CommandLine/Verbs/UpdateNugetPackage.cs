/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;

namespace Nautilus.Cli.Client.CommandLine.Verbs
{
	[Verb("update-nuget-package", HelpText = "Finds the conflicting nuget package versions installed in the solution.")]
	public class UpdateNugetPackage
	{
		private const string Example_Text = "Update nuget package to a specificed verison number";

		[Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file.")]
		public string SolutionFileName { get; set; }

		[Option("project", Required = true, HelpText = "The name of the project to update the nuget packages involved. Use 'all' to upgrade all projects")]
		public string ProjectName { get; set; }

		[Option("package", Required = true, HelpText = "The nuget package name to update.")]
		public string NugetPackage { get; set; }

		[Option("version", Required = true, HelpText = "The version number to upate to.")]
		public string NugetVersion { get; set; }

        [Option("debug", Default = false, Required = false, Hidden = true, HelpText = "Show debugging message including exception message and stacktrace")]
        public bool Debug { get; set; }

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

				var cmd = new UpdateNugetPackage
				{
					NugetPackage = "Xamarin.Forms",
					NugetVersion = "3.6.1.21221121",
					ProjectName = "MyProject.Name",
					SolutionFileName = platformPathSample
				};

				return new List<Example>
				{
					new Example(Example_Text, cmd)
				};
			}
		}
	}
}