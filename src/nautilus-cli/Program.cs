using System;
using System.Diagnostics;
using System.Drawing;
using CommandLine;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core;
using Nautilus.Cli.Core.TestData;
using Nautilus.Cli.Core.Components;
using Nautilus.Cli.Client.CommandLine;
using Nautilus.Cli.Client.CLIServices;

namespace Nautilus.Cli.Client
{
	class Program
	{
		static void Main(string[] args)
		{

			RegisterApplicationComponents();

#if DEBUG
			TestDataHelper.UseTestData = false;
#endif

			_ = Parser.Default.ParseArguments<ListProjects, FindConflict>(args)
				.WithParsed((Action<ListProjects>)((command) =>
				{
					var sw = new Stopwatch();
					sw.Start();

					var service = new ListProjectsService(command.SolutionFileName, command.ProjectsOnly, command.ShowNugetPackages);

					try
					{
						service.Run();
					}
					catch (SolutionFileException solutionFileEx)
					{
						SolutionFileExceptionMessageFormat(solutionFileEx);

						sw.Stop();
						DisplayFinishingMessage(sw);
					}
					catch (Exception ex)
					{
						DisplayGeneralExceptionMessageFormat(ex);

						sw.Stop();
						DisplayFinishingMessage(sw);
					}
					finally
					{
						if (sw.IsRunning)
						{
							sw.Stop();
							DisplayFinishingMessage(sw);
						}
					}
				}))
				.WithParsed((Action<FindConflict>)((command) =>
				{
					var sw = new Stopwatch();
					sw.Start();

					var service = new FindConflictService(command.SolutionFileName, command.Project, command.UseDebugData);

					try
					{
						service.Run();
					}
					catch (SolutionFileException solutionFileEx)
					{
						SolutionFileExceptionMessageFormat(solutionFileEx);

						sw.Stop();
						DisplayFinishingMessage(sw);
					}
					catch (Exception ex)
					{
						DisplayGeneralExceptionMessageFormat(ex);

						sw.Stop();
						DisplayFinishingMessage(sw);
					}
					finally
					{
						if (sw.IsRunning)
						{
							sw.Stop();
							DisplayFinishingMessage(sw);
						}
					}
				}))
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

		private static void DisplayGeneralExceptionMessageFormat(Exception ex)
		{
			Console.WriteLine("");
			Colorful.Console.WriteLine(ex.Message, Color.Red);
			Console.WriteLine("");
			Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

		private static void DisplayCLIExceptionMessageFormat(CLIException cliEx)
		{
			Console.WriteLine("");
			Colorful.Console.WriteLine(cliEx.Message, Color.Red);
			Console.WriteLine("");
			Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

		private static void SolutionFileExceptionMessageFormat(SolutionFileException solutionFileEx)
		{
			Console.WriteLine("");
			Colorful.Console.WriteLine(solutionFileEx.Message, Color.Red);
			Console.WriteLine("");
			Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

		private static void DisplayFinishingMessage(Stopwatch sw)
		{
			Colorful.Console.WriteLine("\nCompleted successfully", Color.GreenYellow);
			Colorful.Console.WriteLine($"execution time : {sw.Elapsed.TotalSeconds} secs\n", Color.GreenYellow);
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