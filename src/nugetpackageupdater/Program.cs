using System.Text;
using Colorful;
using CommandLine;
using NugetPckgUpdater.CommandLine;

namespace SolutionNugetPackagesUpdater
{
	class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<ReportOptions, FindConflict>(args)
				.WithParsed<ReportOptions>((obj) =>
				{
					Console.WriteLine($"ReportOptions : {obj.Path}");
				})
				.WithParsed<FindConflict>((obj) =>
				{
					Console.WriteLine($"FindConflict : {obj.FileName}");
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