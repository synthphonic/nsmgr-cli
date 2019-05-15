using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SolutionNugetPackagesUpdater.Core.FileReaders;
using SolutionNugetPackagesUpdater.Core.Helpers;
using SolutionNugetPackagesUpdater.Core.Models;

namespace SolutionNugetPackagesUpdater.Core.Services
{
	public class FindConflictService
	{
		private string _solutionFileName;
		private readonly bool _processProjectsOnly;

		public FindConflictService(string solutionFileName, bool processProjectsOnly = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_solutionFileName = solutionFileName;
		}

		public void Run()
		{
			var slnFileReader = new SolutionFileReader(_solutionFileName, _processProjectsOnly);
			var solution = slnFileReader.Read();

			//WriteToScreen(solution);

			var foundConflicts = FindConflicts(solution);

			WriteOutput(foundConflicts);
		}

		private void WriteOutput(Dictionary<string, IList<NugetPackageReferenceExtended>> foundConflicts)
		{
			if (!foundConflicts.Any())
				Console.WriteLine("Great! No conflict found for this solution");

			Colorful.Console.WriteLine();
			Colorful.Console.Write("Found ");
			Colorful.Console.Write($"{foundConflicts.Count()} ", Color.Aqua);
			Colorful.Console.WriteLine("Nuget Package conflicts...");
			Colorful.Console.WriteLine();

			foreach (var conflict in foundConflicts)
			{
				Colorful.Console.Write($"Nuget Package : ");
				Colorful.Console.WriteLine($"{conflict.Key}", Color.Aqua);

				foreach (var item in conflict.Value)
				{
					Colorful.Console.Write($"In Project ");
					Colorful.Console.Write("{0,-30}", Color.Azure, item.ProjectName);
					Colorful.Console.Write("[{0,-16}]", Color.Azure, item.ProjectTargetFramework);
					Colorful.Console.Write(" found version ");
					Colorful.Console.Write("{0}", Color.Azure, item.Version);
					Colorful.Console.WriteLine();
				}

				Console.WriteLine();
			}
		}

		private static Dictionary<string, IList<NugetPackageReferenceExtended>> FindConflicts(Solution solution)
		{
			var instance = AppFactory.GetRequestor<IApplicationComponent>(AppComponentType.NugetConflicts.ToString());
			instance.Initialize(solution);
			var result = instance.Execute();

			return result as Dictionary<string, IList<NugetPackageReferenceExtended>>;
		}

		private static void WriteToScreen(Solution solution)
		{
			var projectCounter = 1;
			foreach (var project in solution.Projects)
			{
				Colorful.Console.WriteLine($"{projectCounter}. {project.ProjectName} {project.GetHashCode()}", Color.YellowGreen);
				Colorful.Console.WriteLine($"Found {project.Packages.Count()} nuget packages", Color.YellowGreen);

				foreach (var package in project.Packages)
				{
					Colorful.Console.WriteLine($"{package.PackageName} {package.Version}", Color.Chocolate);
				}

				projectCounter++;
				Colorful.Console.WriteLine();
			}
		}
	}
}