using Nautilus.Cli.Core.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Nautilus.Cli.Core;
using Nautilus.Cli.Core.Components.Http;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.TestData;
using System.Threading;
using Nautilus.Cli.Client.CommandLine.Layout;

namespace Nautilus.Cli.Client.CLIServices
{
	public class FindConflictService
	{
		private string _solutionFileName;
		private readonly bool _processProjectsOnly;

		public FindConflictService(string solutionFileName, bool processProjectsOnly = false, bool debugData = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_solutionFileName = solutionFileName;
			TestDataHelper.UseTestData = debugData;
		}

		public async Task Run()
		{
			Colorful.Console.WriteLine();
			Colorful.Console.Write("Working. Please wait...", Color.DeepSkyBlue);

			var slnFileReader = new SolutionFileReader(_solutionFileName, _processProjectsOnly);
			var solution = slnFileReader.Read();

			var foundConflicts = FindConflicts(solution);

			var packageNames = new List<string>();
			foreach (var item in foundConflicts)
			{
				packageNames.Add(item.Key);
			}

			var latestPackages = await QueryNugetPackageOnlineAsync(packageNames.ToArray());

			Colorful.Console.Write("Done.", Color.DeepSkyBlue);
			Thread.Sleep(500);

			Colorful.Console.WriteLine();

			WriteOutput(foundConflicts, solution.SolutionFileName, solution.Projects.Count(), latestPackages);
		}

		private void WriteOutput(Dictionary<string, IList<NugetPackageReferenceExtended>> foundConflicts, string solutionFileName, int totalProjects, Dictionary<string, string> latestPackages)
		{
			Colorful.Console.WriteLine();
			Colorful.Console.Write("{0,-15}", "Solution ");
			Colorful.Console.WriteLine($": {solutionFileName}", Color.PapayaWhip);
			Colorful.Console.Write("Total Projects : ");
			Colorful.Console.WriteLine($"{totalProjects}", Color.PapayaWhip);

			Colorful.Console.WriteLine();
			Colorful.Console.Write("Found ", Color.PapayaWhip);
			Colorful.Console.Write($"{foundConflicts.Count()} ", Color.Aqua);
			Colorful.Console.WriteLine("Nuget Package conflicts...", Color.PapayaWhip);
			Colorful.Console.WriteLine();

			if (!foundConflicts.Any())
			{
				Colorful.Console.WriteLine("** Great! No conflict found for this solution **", Color.DeepSkyBlue);
			}

			foreach (var conflict in foundConflicts)
			{
				Colorful.Console.Write($"Nuget Package : ");
				Colorful.Console.WriteLine($"{conflict.Key}", Color.Aqua);

				foreach (var item in conflict.Value)
				{
					Colorful.Console.Write($"In Project ");
					Colorful.Console.Write(CliStringFormatter.Format40, Color.Azure, item.ProjectName);
					Colorful.Console.Write("[{0,-16}]", Color.Azure, item.ProjectTargetFramework);
					Colorful.Console.Write(" found version ");
					Colorful.Console.Write("{0}", Color.Azure, item.Version);
					Colorful.Console.WriteLine();
				}

				var latestVersion = latestPackages[conflict.Key];
				if (latestVersion.Contains("Unable"))
				{
					Colorful.Console.Write("Latest nuget package online : ", Color.Goldenrod);
					Colorful.Console.WriteLine("{0}", Color.Red, latestVersion);
				}
				else
				{
					Colorful.Console.WriteLine("*Latest nuget package online : {0}", Color.Goldenrod, latestVersion);
				}

				Colorful.Console.WriteLine();
			}
		}

		private static Dictionary<string, IList<NugetPackageReferenceExtended>> FindConflicts(Solution solution)
		{
			var instance = AppFactory.GetRequestor<IApplicationComponent>(AppComponentType.NugetConflicts.ToString());
			instance.Initialize(solution);
			var result = instance.Execute();

			return result as Dictionary<string, IList<NugetPackageReferenceExtended>>;
		}

		private async Task<Dictionary<string, string>> QueryNugetPackageOnlineAsync(string[] packageNameList)
		{
			var result = new Dictionary<string, string>();

			foreach (var packageName in packageNameList)
			{
				var request = NugetPackageHttpRequest.QueryRequest(packageName, false);
				var response = await request.ExecuteAsync();

				if (response.Exception != null)
				{
					result[packageName] = "Unable to connect to the internet";
				}
				else
				{
					var packageVersion = response.GetCurrentVersion(packageName);
					result[packageName] = packageVersion;
				}
			}

			return result;
		}
	}
}