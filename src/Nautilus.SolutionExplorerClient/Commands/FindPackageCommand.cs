/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("find-package", HelpText = "Finds the project(s) that depends on the intended nuget package")]
class FindPackageCommand : CommandBase
{
    private const string Example_Text = "Finds the project(s) that depends on the intended nuget package";
    private readonly bool _returnResults;

    [Option(longName: "solutionfile", shortName: 's', Required = true, HelpText = "The full file path to the .sln file.")]
    public string SolutionFileName { get; set; }

    [Option(longName: "nuget-package", shortName: 'g', Required = true, HelpText = "The nuget package to find.")]
    public string NugetPackage { get; set; }

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
                    new Example(Example_Text, new FindPackageCommand{ SolutionFileName=platformPathSample, NugetPackage="Xamarin.Forms"} )
                };
        }
    }

    public FindPackageCommand()
    {
        // used internally by CommandLine Parser
    }

    internal FindPackageCommand(string solutionFileName, string nugetPackage, bool returnResults = false)
    {
        SolutionFileName = solutionFileName;
        NugetPackage = nugetPackage;
        _returnResults = returnResults;
    }

    public async Task Run()
    {
        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        var slnFileReader = new SolutionFileReader(SolutionFileName, true);
        var solution = slnFileReader.Read();

        var flatList = solution.ExtractNugetPackageAsFlatList();
        var categorizedByPackageNameList = solution.CategorizeByPackageName(flatList);

        Colorful.Console.WriteLine();

        if (categorizedByPackageNameList.ContainsKey(NugetPackage))
        {
            var projects = categorizedByPackageNameList[NugetPackage];

            if (_returnResults)
            {
                Results = projects;
            }
            else
            {
                WriteOutput(projects, solution.SolutionFileName);
            }

            return;
        }

        Colorful.Console.WriteLine($"'{NugetPackage}' nuget package is not referenced in any project.\n");

        await Task.Delay(1);
    }

    private void WriteOutput(IList<NugetPackageReferenceExtended> projects, string solutionFileName)
    {
        Colorful.Console.WriteLine();
        Colorful.Console.Write("{0,-15}", "Solution ");
        Colorful.Console.WriteLine($": {solutionFileName}", Color.PapayaWhip);

        Colorful.Console.WriteLine();

        Colorful.Console.WriteLine($"{NugetPackage}", Color.Aqua);

        foreach (var project in projects)
        {
            Colorful.Console.Write($"In Project ");
            Colorful.Console.Write(CliStringFormatter.Format40, Color.Azure, project.ProjectName);
            Colorful.Console.Write("[{0,-16}]", Color.Azure, project.ProjectTargetFramework);
            Colorful.Console.Write(" found version ");
            Colorful.Console.Write("{0}", Color.Azure, project.Version);
            Colorful.Console.WriteLine();
        }
    }

    public IList<NugetPackageReferenceExtended> Results { get; private set; }

    internal static void Execute(FindPackageCommand command)
    {
        bool debugMode = command.Debug;
        bool exceptionRaised = false;

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
