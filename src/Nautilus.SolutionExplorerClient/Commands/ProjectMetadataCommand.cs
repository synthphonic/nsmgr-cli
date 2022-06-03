namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("project-metadata", HelpText = CommandHelpText)]
class ProjectMetadataCommand : CommandBase
{
    private const string CommandHelpText = "Add/Delete/Update xml element or csproj metadata in project file";
    private const string AddAuthorsExample = "-Add/Update the 'Authors' project metadata";
    private const string AddVersionExample = "-Add/Update the 'Version' project metadata";
    private const string DeleteExample = "-Delete project metadata";

    #region Command Line Setup & Options
    [Option(longName: "project-path", shortName: 'p', Required = true, HelpText = "The full file path of a given csproj file name.")]
    public string ProjectFilePath { get; set; }

    [Option(longName: "xml-metadata", shortName: 'm', Required = true, HelpText = "The fully qualified metadata or xml element name to operate on")]
    public string Metadata { get; set; }

    [Option(longName: "remove", shortName: 'r', Default = false, Required = false, HelpText = "The metadata or property name to remove")]
    public bool RemoveMetadata { get; set; }

    [Usage()]
    public static IEnumerable<Example> Examples
    {
        get
        {
            var platformPathSample = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platformPathSample = "/projects/myproject/xxx.csproj";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platformPathSample = @"C:\projects\myproject\xxx.csproj";
            }

            var unParserSettings = new UnParserSettings
            {
                GroupSwitches = true,
                PreferShortName = true,
                UseEqualToken = false
            };

            return new List<Example>
                {
                    new Example(AddAuthorsExample, unParserSettings, new ProjectMetadataCommand
                    {
                        ProjectFilePath = platformPathSample,
                        Metadata = "PropertyGroup:Authors=aa,bb,cc"
                    }),
                    new Example(AddVersionExample, unParserSettings, new ProjectMetadataCommand
                    {
                        ProjectFilePath = platformPathSample,
                        Metadata = "PropertyGroup:Version=1.0.0.200"
                    }),
                    new Example(DeleteExample, unParserSettings, new ProjectMetadataCommand
                    {
                        ProjectFilePath = platformPathSample,
                        Metadata = "PropertyGroup:Authors",
                        RemoveMetadata = true
                    })
                };
        }
    }
    #endregion
    public ProjectMetadataCommand()
    {
    }

    public override void Execute()
    {
        Program.DisplayProductInfo();

        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        //
        // project-property -p ../../../../../../nautilus/Nautilus.IO.csproj -t PropertyGroup:Authors=aa,bb,cc
        //

        // Parse the element name and value
        var elementInfo = XmlParser.ParseFromCliParameter(Metadata);

        var prjMetadata = ProjectMetadata.SetMetadata(ProjectFilePath);
        var project = new Project(prjMetadata);
        project.Read();

        if (RemoveMetadata)
        {
            project
                .ParentElement(elementInfo.SearchElement)
                .Remove(elementInfo.TargetElement);
        }
        else
        {
            project
                .ParentElement(elementInfo.SearchElement)
                .Create(elementInfo.TargetElement, elementInfo.TargetValue);
        }
    }
}