using System;
using System.Diagnostics;
using System.Drawing;
using CommandLine;
using NugetPckgUpdater.CommandLine;
using SolutionNugetPackagesUpdater.Core;
using SolutionNugetPackagesUpdater.Core.Components;
using SolutionNugetPackagesUpdater.Core.Exceptions;
using SolutionNugetPackagesUpdater.Core.Services;

namespace SolutionNugetPackagesUpdater
{
	class Program
	{
		static void Main(string[] args)
		{

			RegisterApplicationComponents();

#if DEBUG
			TestData.TestDataHelper.UseTestData = true;
#endif

			_ = Parser.Default.ParseArguments<ReportOptions, FindConflict>(args)
				.WithParsed<ReportOptions>((command) =>
				{
					Colorful.Console.WriteLine($"ReportOptions : {command.Path}", Color.Green);
				})
				.WithParsed<FindConflict>((command) =>
				{
					try
					{
						var sw = new Stopwatch();
						sw.Start();

						var service = new FindConflictService(command.SolutionFileName, command.Project, command.UseDebugData);
						service.Run();
						sw.Stop();

						Colorful.Console.WriteLine("\nCompleted successfully", Color.GreenYellow);
						Colorful.Console.WriteLine($"execution time : {sw.Elapsed.TotalSeconds} secs\n", Color.GreenYellow);
					}
					catch (SolutionFileException solutionFileEx)
					{
						Console.WriteLine("");
						Colorful.Console.WriteLine(solutionFileEx.Message, Color.Red);
						Console.WriteLine("");
						Colorful.Console.WriteLine("Program has stopped", Color.Red);
						Console.WriteLine("");
					}
					catch (CLIException cliEx)
					{
						Console.WriteLine("");
						Colorful.Console.WriteLine(cliEx.Message, Color.Red);
						Console.WriteLine("");
						Colorful.Console.WriteLine("Program has stopped", Color.Red);
						Console.WriteLine("");
					}
					catch (Exception ex)
					{
						Console.WriteLine("");
						Colorful.Console.WriteLine(ex.Message, Color.Red);
						Console.WriteLine("");
						Colorful.Console.WriteLine("Program has stopped", Color.Red);
						Console.WriteLine("");
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

		private static void RegisterApplicationComponents()
		{
			AppFactory.Register<NugetPackageConflictFinder>(AppComponentType.NugetConflicts.ToString());
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