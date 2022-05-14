/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("list-projects", HelpText = "List out all projects that exists under a solution (.sln) file")]
class ListProjectsCommand
{
    private const string Example_Text = "List out all projects under the solution";

    [Option(longName: "solutionfile", shortName: 's', Required = true, HelpText = "The full file path to the .sln file.")]
    public string SolutionFileName { get; set; }

    [Option(longName: "projects-only", shortName: 'p', HelpText = "Process project files only and ignore the rest.")]
    public bool ProjectsOnly { get; set; }

    [Option(longName: "nuget-packages", shortName: 'n', Default = false, Required = false, Hidden = false, HelpText = "Display nuget packages for each project.")]
    public bool ShowNugetPackages { get; set; }

    [Option(longName: "nuget-package-updates", shortName: 'u', Default = false, Required = false, Hidden = false, HelpText = "Query and display if there's any new nuget package update version online.")]
    public bool NugetPackageUpdates { get; set; }

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
}
