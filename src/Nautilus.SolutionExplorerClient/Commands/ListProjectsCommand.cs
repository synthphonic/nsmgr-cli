/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("list-projects", HelpText = "List out all projects that exists under a solution (.sln) file")]
internal class ListProjectsCommand
{
    private const string Example_Text = "List out all projects under the solution";
    //delegate void ShowNugetPackages(Project project);

    [Option(longName: "solutionfile", shortName: 's', Required = true, HelpText = "The full file path to the .sln file.")]
    public string SolutionFileName { get; set; }

    [Option(longName: "projects-only", shortName: 'p', HelpText = "Process project files only and ignore the rest.")]
    public bool ProjectsOnly { get; set; }

    [Option(longName: "nuget-packages", shortName: 'n', Default = false, Required = false, Hidden = false, HelpText = "Display nuget packages for each project.")]
    public bool ShowNugetPackages { get; set; }

    [Option(longName: "nuget-package-updates", shortName: 'u', Default = false, Required = false, Hidden = false, HelpText = "Query and display if there's any new nuget package update version online.")]
    public bool ShowNugetPackageUpdates { get; set; }

    [Option(longName: "debug", shortName: 'd', Default = false, Required = false, Hidden = true, HelpText = "Show debugging message including exception message and stacktrace")]
    public bool Debug { get; set; }

    [Usage(ApplicationAlias = Program.CliName)]
    public static IEnumerable<Example> Examples
    {
        get
        {
            var platformPathSample = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platformPathSample = "/users/itsme/xxx.sln";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platformPathSample = @"C:\myproject\xxx.sln";
            }

            return new List<Example>
                {
                    new Example(Example_Text, new ListProjectsCommand{ SolutionFileName=platformPathSample} )
                };
        }
    }

    public ListProjectsCommand()
    {

    }

    internal async Task Run()
    {
        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.DeepSkyBlue);

        var slnFileReader = new SolutionFileReader(SolutionFileName, ProjectsOnly);
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
        if (ShowNugetPackageUpdates)
        {
            comparer = new NugetPackageOnlineComparer(solution, CliStringFormatter.WriteOnlinePackageProgressHandler);
            await comparer.Run();
        }

        Colorful.Console.WriteLine();

        WriteToScreen(solution, comparer?.Result);
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
                Colorful.Console.Write($"[{CliStringFormatter.Format7}]", Color.Chocolate, project.TargetFramework);
                Colorful.Console.WriteLine();

                if (project.TargetFramework == ProjectTarget.Unknown)
                {

                }

                if (ShowNugetPackages)
                {
                    DisplayNugetPackages(project, packageVersionComparer);
                    Colorful.Console.WriteLine();
                }
            }
            else
            {
                Colorful.Console.Write(CliStringFormatter.Format45, Color.YellowGreen, $"{project.ProjectName}");

                if (project.TargetFramework == ProjectTarget.Unknown)
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
            else
            {
                Colorful.Console.WriteLine();
            }
        }
    }
}
