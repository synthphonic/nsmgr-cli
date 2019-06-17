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
using Nautilus.Cli.Client.CommandLine.Layout;

namespace Nautilus.Cli.Client.CommandLine.Services
{
	public class ListProjectsService
	{
		delegate void ShowNugetPackages(Project project);

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
				comparer = new NugetPackageOnlineComparer(solution, CliStringFormatter.WriteOnlinePackageProgressHandler);
				await comparer.Run();
			}

			Colorful.Console.WriteLine();

			WriteToScreen(solution,comparer?.Result);
		}

		private void WriteToScreen(Solution solution, Dictionary<string, IList<NugetPackageInformationComparer>> packageVersionComparer)
		{
			Colorful.Console.WriteLine();
			Colorful.Console.Write($"{CliStringFormatter.Format15}", "Solution ");
			Colorful.Console.WriteLine($": {solution.SolutionFileName}", Color.PapayaWhip);
			Colorful.Console.Write("Total Projects : ");
			Colorful.Console.WriteLine($"{solution.Projects.Count()}\n", Color.PapayaWhip);

			var projectCounter = 1;
			foreach (var project in solution.Projects)
			{
				Colorful.Console.Write($"{CliStringFormatter.Format1}. ", projectCounter);
                if (project.ProjectType == SolutionProjectElement.CSharpProject)
                {
                    Colorful.Console.Write(CliStringFormatter.Format45, Color.YellowGreen, $"{project.ProjectName} ({project.Packages.Count()})");
                }
                else
                {
                    Colorful.Console.Write(CliStringFormatter.Format45, Color.YellowGreen, $"{project.ProjectName}");
                }

				if (project.TargetFramework == ProjectTarget.Unknown)
				{
					Colorful.Console.Write($"[{CliStringFormatter.Format7}-", Color.Chocolate, project.TargetFramework);
					Colorful.Console.WriteLine($"{CliStringFormatter.Format9}]", Color.Chocolate, project.ProjectType);
				}
				else
				{
					Colorful.Console.WriteLine($"[{CliStringFormatter.Format5}]", project.TargetFramework);
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
				Colorful.Console.Write($"\t{CliStringFormatter.Format40}", Color.Chocolate, package.PackageName);
				Colorful.Console.Write($"{CliStringFormatter.Format30}", Color.YellowGreen, package.Version);

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

					Colorful.Console.WriteLine($"{CliStringFormatter.Format5}", color, latestVersionMessage);
				}
			}
		}
	}
}