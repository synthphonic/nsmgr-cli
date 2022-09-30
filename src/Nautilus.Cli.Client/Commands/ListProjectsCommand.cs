/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.Cli.Client.Commands;

internal sealed class ListProjectsCommand
{
    private readonly string _solutionFileName;
    private readonly bool _projectsOnly;
    private readonly bool _showNugetUpdates;
    private readonly bool _showPreReleaseNugetPackages;

    public ListProjectsCommand(string solutionFileName, bool projectsOnly, string nugetPackage, bool showNugetUpdates, bool showPreReleaseNugetPackages, bool returnResults = false)
    {
        _solutionFileName = solutionFileName;
        _projectsOnly = projectsOnly;
        _showNugetUpdates = showNugetUpdates;
        _showPreReleaseNugetPackages = showPreReleaseNugetPackages;
    }

    public void Execute()
    {
        Run().Wait();
    }

    private async Task Run()
    {
        var slnFileReader = new SolutionFileReader(_solutionFileName, _projectsOnly);
        Solution solution = null!;

        #region Solution file reader execution section
        try
        {
            solution = slnFileReader.Read();

            Colorful.Console.WriteLine();
            Colorful.Console.Write($"{CliStringFormatter.Format15}", "Solution ");
            Colorful.Console.WriteLine($": {solution.SolutionFileName}", Color.PapayaWhip);
            Colorful.Console.Write("Total Projects : ");
            Colorful.Console.WriteLine($"{solution.Projects.Count()}\n", Color.PapayaWhip);

            Colorful.Console.WriteLine();
            Colorful.Console.WriteLine("Working. Please wait...", Color.DeepSkyBlue);
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

        NugetPackageOnlineComparer comparer = null!;
        if (_showNugetUpdates)
        {
            comparer = new NugetPackageOnlineComparer(solution, _showPreReleaseNugetPackages, CliStringFormatter.WriteOnlinePackageProgressHandler);
            await comparer.Run();
        }

        Colorful.Console.WriteLine();

        WriteFinalizedResultToScreen(solution, comparer?.Result!);
    }

    private void WriteFinalizedResultToScreen(Solution solution, Dictionary<string, IList<NugetPackageInformationComparer>> packageVersionComparer)
    {
        var projectCounter = 1;
        foreach (var project in solution.Projects)
        {
            Colorful.Console.Write($"{CliStringFormatter.Format1}. ", projectCounter);

            if (project.ProjectType == SolutionProjectElement.CSharpProject)
            {
                Colorful.Console.Write(CliStringFormatter.Format54, Color.YellowGreen, $"{project.ProjectName} ({project.Packages.Count()})");
                Colorful.Console.Write($"[{CliStringFormatter.Format5}]", Color.Chocolate, project.TargetFramework);
                Colorful.Console.WriteLine();

                if (project.TargetFramework == ProjectTargetFramework.Unknown)
                {
                    // Todo: do something here? 
                }

                if (_showNugetUpdates)
                {
                    DisplayNugetPackages(project, packageVersionComparer);
                    Colorful.Console.WriteLine();
                }
            }
            else
            {
                Colorful.Console.Write(CliStringFormatter.Format45, Color.YellowGreen, $"{project.ProjectName}");

                if (project.TargetFramework == ProjectTargetFramework.Unknown)
                {
                    Colorful.Console.Write($"[{CliStringFormatter.Format7}-", Color.Chocolate, project.TargetFramework);
                    Colorful.Console.WriteLine($"{CliStringFormatter.Format9}]", Color.Chocolate, project.ProjectType);
                }
                else
                {
                    Colorful.Console.WriteLine($"[{CliStringFormatter.Format5}]", project.TargetFramework);
                }
            }

            projectCounter++;
        }
    }

    private static void DisplayNugetPackages(Project project, Dictionary<string, IList<NugetPackageInformationComparer>> packageVersionComparer = null!)
    {
        foreach (var package in project.Packages)
        {
            Colorful.Console.Write($"\t{CliStringFormatter.Format50}", Color.Chocolate, package.PackageName);
            Colorful.Console.Write($"{CliStringFormatter.Format30}", Color.YellowGreen, package.Version);

            if (packageVersionComparer != null)
            {
                var nugetPackageInformation = packageVersionComparer[project.ProjectName].FirstOrDefault(x => x.PackageName.Equals(package.PackageName));

                // if nugetPackageInformation is null, then try ToLower() the PackageName, then find it again
                if (nugetPackageInformation == null)
                {
                    nugetPackageInformation = packageVersionComparer[project.ProjectName].FirstOrDefault(x => x.PackageName.ToLower().Equals(package.PackageName));
                }

                if (nugetPackageInformation == null) continue;

                var latestVersionMessage = string.Empty;
                Color color = Color.Green;

                if (nugetPackageInformation.OnlinePackageExists)
                {
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
                }
                else
                {
                    latestVersionMessage = $"[Skipped]";
                    color = Color.Yellow;
                }

                Colorful.Console.WriteLine($"{CliStringFormatter.Format5}", color, latestVersionMessage);
            }
            else
            {
                Colorful.Console.WriteLine();
            }
        }
    }
}