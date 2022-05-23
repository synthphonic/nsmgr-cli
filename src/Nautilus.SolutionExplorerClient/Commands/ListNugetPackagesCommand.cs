/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("list-packages", HelpText = "List nuget packages for all projects in the solution")]
class ListNugetPackagesCommand : CommandBase
{
    private const string Example_Text = "Finds any conflicting nuget package versions in the solution";

    [Option(longName: "solutionfile", shortName: 's', Required = true, HelpText = "The full file path to the .sln file.")]
    public string SolutionFileName { get; set; }

    [Option("projects-only", Default = true, Required = false, Hidden = true, HelpText = "Process project files only and ignore the rest. Default is false")]
    public bool ProjectsOnly { get; set; }

    [Option("usedebugdata", Required = false, Hidden = true, HelpText = "")]
    public bool UseDebugData { get; set; }

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
                    new Example(Example_Text, new ListNugetPackagesCommand{ SolutionFileName=platformPathSample} )
                };
        }
    }

    public ListNugetPackagesCommand()
    {

    }

    //public ListNugetPackagesService(string solutionFileName, bool processProjectsOnly = false, bool debugData = false)
    //{
    //    _processProjectsOnly = processProjectsOnly;
    //    _solutionFileName = solutionFileName;
    //    TestDataHelper.UseTestData = debugData;
    //}

    public async Task Run()
    {
        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        var slnFileReader = new SolutionFileReader(SolutionFileName, ProjectsOnly);
        var solution = slnFileReader.Read();

        var flatList = solution.ExtractNugetPackageAsFlatList();
        var categorizedByPackageNameList = solution.CategorizeByPackageName(flatList);

        var packageNames = new List<string>();
        foreach (var item in categorizedByPackageNameList)
        {
            packageNames.Add(item.Key);
        }

        var latestPackages = await QueryNugetPackageOnlineAsync(packageNames.ToArray(), CliStringFormatter.WriteOnlinePackageProgressHandler);

        Colorful.Console.WriteLine();

        categorizedByPackageNameList = categorizedByPackageNameList.OrderBy(x => x.Key).ToDictionary();

        WriteOutput(categorizedByPackageNameList, solution.SolutionFileName, solution.Projects.Count(), latestPackages);
    }

    private void WriteOutput(Dictionary<string, IList<NugetPackageReferenceExtended>> nugetPackages, string solutionFileName, int totalProjects, Dictionary<string, string> latestPackages)
    {
        Colorful.Console.WriteLine();
        Colorful.Console.Write("{0,-15}", "Solution ");
        Colorful.Console.WriteLine($": {solutionFileName}", Color.PapayaWhip);
        Colorful.Console.Write("Total Projects : ");
        Colorful.Console.WriteLine($"{totalProjects}", Color.PapayaWhip);

        Colorful.Console.WriteLine();
        Colorful.Console.Write("Found ", Color.PapayaWhip);
        Colorful.Console.Write($"{nugetPackages.Count()} ", Color.Aqua);
        Colorful.Console.WriteLine("Nuget Packages...", Color.PapayaWhip);
        Colorful.Console.WriteLine();

        if (!nugetPackages.Any())
        {
            Colorful.Console.WriteLine("** Great! No conflict found for this solution **", Color.DeepSkyBlue);
        }

        foreach (var package in nugetPackages)
        {
            Colorful.Console.WriteLine($"{package.Key}", Color.Aqua);

            var sortedByProjects = package.Value.OrderBy(x => x.ProjectName).ToList();

            foreach (var item in sortedByProjects)
            {
                Colorful.Console.Write($"In Project ");
                Colorful.Console.Write(CliStringFormatter.Format40, Color.Azure, item.ProjectName);
                Colorful.Console.Write("[{0,-16}]", Color.Azure, item.ProjectTargetFramework);
                Colorful.Console.Write(" found version ");
                Colorful.Console.Write("{0}", Color.Azure, item.Version);
                Colorful.Console.WriteLine();
            }

            var latestVersion = latestPackages[package.Key];
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

    private async Task<Dictionary<string, string>> QueryNugetPackageOnlineAsync(string[] packageNameList, Action writeProgressHandler = null)
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

            writeProgressHandler?.Invoke();
        }

        return result;
    }

    internal static void Execute(ListNugetPackagesCommand command)
    {
        bool debugMode = command.Debug;
        bool exceptionRaised = true;

        var sw = new Stopwatch();
        sw.Start();

        try
        {
            command.Run().Wait();
        }
        catch (CLIException cliEx)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.DisplayCLIExceptionMessageFormat(cliEx, CLIConstants.LogFileName, debugMode);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        catch (SolutionFileException solutionFileEx)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.SolutionFileExceptionMessageFormat(solutionFileEx);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        catch (Exception ex)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.DisplayGeneralExceptionMessageFormat(ex, CLIConstants.LogFileName, debugMode);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        finally
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }

            ConsoleMessages.DisplayExecutionTimeMessage(sw);

            if (!exceptionRaised)
            {
                ConsoleMessages.DisplayCompletedSuccessfullyFinishingMessage();
            }
        }
    }
}
