namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("modify-project-version", HelpText = Example_Text)]
class ModifyProjectVersionCommand : CommandBase
{
    private const string Example_Text = "Modify a particular C# project 'Version' element accordingly";

    #region Command Line Setup & Options
    [Option(longName: "project-path", shortName: 'p', Required = true, HelpText = "The full file path of a given csproj file name.")]
    public string ProjectFilePath { get; set; }

    [Option(longName: "version-number", shortName: 'v', Required = true, HelpText = "The new version number.")]
    public string VersionNumber { get; set; }

    [Option(longName: "backup", shortName: 'b', Default = false, Required = false, HelpText = "Prior to version change, the command should backup the original version number.")]
    public bool Backup { get; set; }

    [Option(longName: "restore-version", shortName: 'r', Default = false, Required = false, HelpText = "Restore the version number to its original state.")]
    public bool RestoreVersionNumber { get; set; }

    [Usage()]
    public static IEnumerable<Example> Examples
    {
        get
        {
            var platformPathSample = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platformPathSample = "/users/itsme/myproject/xxx.csproj";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platformPathSample = @"C:\users\myproject\xxx.csproj";
            }

            return new List<Example>
                {
                    new Example(Example_Text, new ModifyProjectVersionCommand
                    {
                        ProjectFilePath = platformPathSample,
                        VersionNumber = "3.21.2",
                        Backup = true
                    }),
                    new Example(Example_Text, new ModifyProjectVersionCommand
                    {
                        ProjectFilePath = platformPathSample,
                         RestoreVersionNumber = true
                    })
                };
        }
    }
    #endregion

    public ModifyProjectVersionCommand()
    {
        // used internally by CommandLine Parser
    }

    private async Task Run()
    {
        Program.DisplayProductInfo();

        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        var prjMetadata = ProjectMetadata.SetMetadata(ProjectFilePath);
        var project = new Project(prjMetadata);
        project.Read();

        if (RestoreVersionNumber && Backup)
            throw new CLIException("Restore and Backup switch cannot be used together in a single command line usage.");

        if (RestoreVersionNumber)
        {
            project.Restore();
        }
        else if (Backup)
        {
            project.Backup();
            project.Update("Version", VersionNumber);
        }

        await Task.CompletedTask;
    }

    public override void Execute()
    {
        Run().Wait();
    }
}