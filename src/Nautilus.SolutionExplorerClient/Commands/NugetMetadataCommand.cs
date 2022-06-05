namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("nuget-metadata", HelpText = "Add/Delete/Update specific nuget metadata for a given project file")]
public class NugetMetadataCommand : CommandBase
{
    private const string ParentElement = "PropertyGroup";
    private const string DescriptionMetadata = "Description";
    private const string AuthorsMetadata = "Authors";
    private const string OwnersMetadata = "Owners";
    private const string PackageTagsMetadata = "PackageTags";
    private const string PackageReadmeFileMetadata = "PackageReadmeFile";
    private const string CopyrightMetadata = "Copyright";
    private const string PackageVersionMetadata = "PackageVersion";

    #region Command Line Setup & Options
    [Option(longName: "project-path", shortName: 'p', Required = true, HelpText = "The full file path of a given csproj file name.")]
    public string ProjectFilePath { get; set; }

    [Option(longName: "remove", shortName: 'r', Default = false, Required = false, HelpText = "Remove nuget package specific metadata from the project file")]
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

            return new List<Example>
                {
                    new Example("-Add nuget metadata for a project", new NugetMetadataCommand {
                        ProjectFilePath=platformPathSample
                    }), 
                    new Example("-Delete nuget metadata for a project", new NugetMetadataCommand {
                        RemoveMetadata=true,
                        ProjectFilePath=platformPathSample
                    })
                };
        }
    }
    #endregion

    public NugetMetadataCommand()
    {
    }


    public override void Execute()
    {
        Program.DisplayProductInfo();

        var prjMetadata = ProjectMetadata.SetMetadata(ProjectFilePath);
        var project = new Project(prjMetadata);
        project.Read();

        if (RemoveMetadata)
        {
            DeleteNugetMetadata(project);
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Nuget Metadata Metadata");
        Console.WriteLine();

        AddNugetMetadata(project);
    }

    private static void AddNugetMetadata(Project project)
    {
        Console.Write("Description: ");
        var description = Console.ReadLine();

        Console.Write("Authors: ");
        var authors = Console.ReadLine();

        Console.Write("Owners: ");
        var owners = Console.ReadLine();

        Console.Write("Package Tags: ");
        var packageTags = Console.ReadLine();

        Console.Write("Package README file name: ");
        var readmeFileName = Console.ReadLine();

        Console.Write("Copyright: ");
        var copyright = Console.ReadLine();

        Console.Write("Package version: ");
        var packageVersion = Console.ReadLine();

        project.ParentElement(ParentElement).Create(DescriptionMetadata, description);
        project.ParentElement(ParentElement).Create(AuthorsMetadata, authors);
        project.ParentElement(ParentElement).Create(OwnersMetadata, owners);
        project.ParentElement(ParentElement).Create(PackageTagsMetadata, packageTags);
        project.ParentElement(ParentElement).Create(PackageReadmeFileMetadata, readmeFileName);
        project.ParentElement(ParentElement).Create(CopyrightMetadata, copyright);
        project.ParentElement(ParentElement).Create(PackageVersionMetadata, packageVersion);
    }

    private static void DeleteNugetMetadata(Project project)
    {
        project.ParentElement(ParentElement).Remove(DescriptionMetadata);
        project.ParentElement(ParentElement).Remove(AuthorsMetadata);
        project.ParentElement(ParentElement).Remove(OwnersMetadata);
        project.ParentElement(ParentElement).Remove(PackageTagsMetadata);
        project.ParentElement(ParentElement).Remove(PackageReadmeFileMetadata);
        project.ParentElement(ParentElement).Remove(CopyrightMetadata);
        project.ParentElement(ParentElement).Remove(PackageVersionMetadata);
    }
}