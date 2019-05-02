using System;
using System.Drawing;
using System.Text;
using Colorful;
using CommandLine;
using NugetPckgUpdater.CommandLine;
using SolutionNugetPackagesUpdater.Core.Exceptions;
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
					Colorful.Console.WriteLine($"ReportOptions : {command.Path}", Color.Green);
				})
				.WithParsed<FindConflict>((command) =>
				{
					try
					{
						var service = new FindConflictService(command.FileName, command.Project);
						service.Run();

						Colorful.Console.WriteLine("\nCompleted successfully\n", Color.GreenYellow);
					}
					catch(CLIException cliEx)
					{
						System.Console.WriteLine("");
						Colorful.Console.WriteLine(cliEx.Message, Color.Red);
						System.Console.WriteLine("");
						Colorful.Console.WriteLine("Program has stopped", Color.Red);
						System.Console.WriteLine("");
					}
					catch (Exception ex)
					{
						System.Console.WriteLine("");
						Colorful.Console.WriteLine(ex.Message, Color.Red);
						System.Console.WriteLine("");
						Colorful.Console.WriteLine("Program has stopped", Color.Red);
						System.Console.WriteLine("");
					}
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