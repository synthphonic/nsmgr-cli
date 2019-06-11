using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.Components;
using System.Collections.Generic;

namespace Nautilus.Cli.Client.CLIServices
{
	public class ListProjectsService
	{
		delegate void ShowNugetPackages(Project project);

		private const string Format1 = "{0,-1}";
		private const string Format5 = "{0,-5}"; 
		private const string Format7 = "{0,-7}";
		private const string Format9 = "{0,-9}";
		private const string Format15 = "{0,-15}";
		private const string Format30 = "{0,-30}";
		private const string Format40 = "{0,-40}";
		private const string Format45 = "{0,-45}";
		private const string Format50 = "{0,-50}";

		private readonly string _solutionFileName;
		private readonly bool _processProjectsOnly;
		private readonly bool _showNugetPackages;
		private readonly bool _showNugetPackageUpdates;

		public ListProjectsService(string solutionFileName, bool processProjectsOnly, bool showNugetPackages = false, bool showNugetPackageUpdates = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_solutionFileName = solutionFileName;
			_showNugetPackages = showNugetPackages;
			_showNugetPackageUpdates = showNugetPackageUpdates;
		}

		internal async Task Run()
		{
			Colorful.Console.WriteLine();
			Colorful.Console.WriteLine("Working. Please wait...", Color.DeepSkyBlue);

			var slnFileReader = new SolutionFileReader(_solutionFileName, _processProjectsOnly);
			Solution solution = null;

			#region Solution file reader execution section
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
			#endregion

			NugetPackageOnlineComparer comparer = null;
			if (_showNugetPackageUpdates)
			{
				comparer = new NugetPackageOnlineComparer(solution, WriteOnlinePackageProgressHandler);
				await comparer.Run();
			}

			Colorful.Console.WriteLine();

			WriteToScreen(solution,comparer?.Result);
		}

		private static void WriteOnlinePackageProgressHandler()
		{
			Task.Run(() => Colorful.Console.Write(".", Color.DeepSkyBlue));
		}

		private void WriteToScreen(Solution solution, Dictionary<string, IList<NugetPackageInformationComparer>> packageVersionComparer)
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

				if (_showNugetPackages)
				{
					DisplayNugetPackages(project, packageVersionComparer);
				}

				projectCounter++;
			}
		}

		private static void DisplayNugetPackages(Project project, Dictionary<string, IList<NugetPackageInformationComparer>> packageVersionComparer = null)
		{
			foreach (var package in project.Packages)
			{
				Colorful.Console.Write($"\t{Format40}", Color.Chocolate, package.PackageName);
				Colorful.Console.Write($"{Format30}", Color.YellowGreen, package.Version);

				if (packageVersionComparer != null)
				{
					var nugetPackageInformation = packageVersionComparer[project.ProjectName].FirstOrDefault(x => x.PackageName.Equals(package.PackageName));
					var latestVersionMessage = string.Empty;
					Color color = Color.Green;

					if (nugetPackageInformation.IsLatestVersion)
					{
						latestVersionMessage = "[OK]";
						color = Color.SeaGreen;
					}
					else
					{
						latestVersionMessage = $"{nugetPackageInformation.OnlineVersion}";
						color = Color.OrangeRed;
					}

					Colorful.Console.WriteLine($"{Format5}", color, latestVersionMessage);
				}
			}
		}
	}
}