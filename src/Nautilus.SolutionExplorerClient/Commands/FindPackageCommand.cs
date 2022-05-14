/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */
namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("find-package", HelpText = "Finds the project(s) that depends on the intended nuget package")]
class FindPackageCommand
{
    private const string Example_Text = "Finds the project(s) that depends on the intended nuget package";

    [Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file")]
    public string SolutionFileName { get; set; }

    [Option("package", Required = true, HelpText = "The nuget package name to find.")]
    public string NugetPackage { get; set; }

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
                    new Example(Example_Text, new FindPackageCommand{ SolutionFileName=platformPathSample, NugetPackage="Xamarin.Forms"} )
                };
        }
    }
}
