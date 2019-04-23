using System.Collections.Generic;
using CommandLine;

namespace NugetPckgUpdater.CommandLine
{
	[Verb("solutionreport", HelpText = "The solution report to display")]
	class ReportOptions
	{
		[Option("path", HelpText = "The file path to write the result. Should include file name as well", Separator = ':')]
		public string Path { get; set; }
	}

	[Verb("findconflict", HelpText = "Finds the conflicting nuget package versions installed in the solution")]
	class FindConflict
	{
		[Option("path", Required = true, HelpText = "The file path to write the result. Should include file name as well", Separator = ':')]
		public string Path { get; set; }
	}
}