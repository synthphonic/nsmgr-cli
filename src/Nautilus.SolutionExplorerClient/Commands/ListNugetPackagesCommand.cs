/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("list-packages", HelpText = "List nuget packages for all projects in the solution")]
class ListNugetPackagesCommand
{
    private const string Example_Text = "Finds any conflicting nuget package versions in the solution";

    [Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file")]
    public string SolutionFileName { get; set; }

    [Option("projects-only", Default = true, Required = false, Hidden = true, HelpText = "Process project files only and ignore the rest. Default is false")]
    public bool ProjectsOnly { get; set; }

    [Option("usedebugdata", Required = false, Hidden = true, HelpText = "")]
    public bool UseDebugData { get; set; }

    [Option("debug", Default = false, Required = false, Hidden = true, HelpText = "Show debugging message including exception message and stacktrace")]
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
                    new Example(Example_Text, new ListNugetPackagesCommand{ SolutionFileName=platformPathSample} )
                };
        }
    }
}
