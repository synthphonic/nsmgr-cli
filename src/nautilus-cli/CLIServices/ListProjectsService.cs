using System;
using System.Drawing;
using System.Linq;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Client.CLIServices
{
	public class ListProjectsService
	{
		delegate void ShowNugetPackages(Project project);

		private const string Format1 = "{0,-1}";
		private const string Format7 = "{0,-7}";
		private const string Format9 = "{0,-9}";
		private const string Format5 = "{0,-5}";
		private const string Format15 = "{0,-15}";
		private const string Format40 = "{0,-40}";
		private const string Format45 = "{0,-45}";
		private const string Format50 = "{0,-50}";
		private readonly string _solutionFileName;
		private readonly bool _processProjectsOnly;
		private readonly bool _showNugetPackages;

		public ListProjectsService(string solutionFileName, bool processProjectsOnly, bool showNugetPackages = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_solutionFileName = solutionFileName;
			_showNugetPackages = showNugetPackages;
		}

		public void Run()
		{
			var slnFileReader = new SolutionFileReader(_solutionFileName, _processProjectsOnly);
			Solution solution = null;

			try
			{
				solution = slnFileReader.Read();
			}
			catch (SolutionFileException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}

			Action<Project> writeToScreenAction = null;
			if (_showNugetPackages)
			{
				writeToScreenAction = ListNugetPackages;
			}

			WriteToScreen(solution, writeToScreenAction);
		}

		private static void WriteToScreen(Solution solution, Action<Project> executeAction = null)
		{
			Colorful.Console.WriteLine();
			Colorful.Console.Write($"{Format15}", "Solution ");
			Colorful.Console.WriteLine($": {solution.SolutionFileName}", Color.PapayaWhip);
			Colorful.Console.Write("Total Projects : ");
			Colorful.Console.WriteLine($"{solution.Projects.Count()}\n", Color.PapayaWhip);

			var projectCounter = 1;
			foreach (var project in solution.Projects)
			{
				Colorful.Console.Write($"{Format1}. ", projectCounter);
				Colorful.Console.Write(Format45, Color.YellowGreen, $"{project.ProjectName} ({project.Packages.Count()})");

				if (project.TargetFramework == ProjectTarget.Unknown)
				{
					Colorful.Console.Write($"[{Format7}-", Color.Chocolate, project.TargetFramework);
					Colorful.Console.WriteLine($"{Format9}]", Color.Chocolate, project.ProjectType);
				}
				else
				{
					Colorful.Console.WriteLine($"[{Format5}]", project.TargetFramework);
				}

				executeAction?.Invoke(project);

				projectCounter++;
			}
		}

		private static void ListNugetPackages(Project project)
		{
			foreach (var package in project.Packages)
			{
				Colorful.Console.Write($"\t{Format50}", Color.Chocolate, package.PackageName);
				Colorful.Console.WriteLine($"{Format40}", Color.YellowGreen, package.Version);
			}
		}
	}
}