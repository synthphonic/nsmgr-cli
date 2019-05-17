using System;
using System.Drawing;
using System.Linq;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;

namespace NautilusCLI.CLIServices
{
	public class ListProjectsService
	{
		delegate void ShowNugetPackages(Project project);

		private const string Format = "{0,-45}";
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
			catch(Exception ex)
			{
				throw ex;
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
			Colorful.Console.Write("{0,-15}", "Solution ");
			Colorful.Console.WriteLine($": {solution.SolutionFileName}", Color.PapayaWhip);
			Colorful.Console.Write("Total Projects : ");
			Colorful.Console.WriteLine($"{solution.Projects.Count()}", Color.PapayaWhip);

			var projectCounter = 1;
			foreach (var project in solution.Projects)
			{
				Colorful.Console.Write("{0,-2}. ",projectCounter);
				Colorful.Console.Write(Format, Color.YellowGreen, project.ProjectName);

				if (project.TargetFramework == ProjectTarget.Unknown)
				{
					Colorful.Console.Write("[{0,-7}-", Color.Chocolate, project.TargetFramework);
					Colorful.Console.WriteLine("{0,-9}]", Color.Chocolate, project.ProjectType);
				}
				else
				{
					Colorful.Console.WriteLine("[{0,-5}]", project.TargetFramework);
				}

				executeAction?.Invoke(project);

				projectCounter++;
			}
		}

		private static void ListNugetPackages(Project project)
		{
			Colorful.Console.WriteLine($"\tFound {project.Packages.Count()} nuget packages", Color.YellowGreen);

			foreach (var package in project.Packages)
			{
				Colorful.Console.WriteLine($"\t{package.PackageName} {package.Version}", Color.Chocolate);
			}
		}
	}
}
