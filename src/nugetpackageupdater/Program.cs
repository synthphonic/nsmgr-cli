using System.Text;
using Colorful;
using CommandLine;
using NugetPckgUpdater.CommandLine;
using SolutionNugetPackagesUpdater.Core.Services;

namespace SolutionNugetPackagesUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<ReportOptions, FindConflict>(args)
				.WithParsed<ReportOptions>((command) =>
				{
					Console.WriteLine($"ReportOptions : {command.Path}");
				})
				.WithParsed<FindConflict>((command) =>
				{
					var service = new FindConflictService(command.FileName);
					service.Run();
				})
				.WithNotParsed(errs =>
				{
					//var sb = new StringBuilder();
					//foreach (var item in errs)
					//{
					//	sb.AppendFormat($"{item.ToString()}");
					//}

					//Console.WriteLine(sb.ToString());
				});
		}

		public const string Name = "mycli";

		//public static readonly string Name
		//{
		//	get
		//	{
		//		var asmName = new FindConflict().GetType().Assembly.GetName();
		//		return asmName.Name;
		//	}
		//}
	}
}