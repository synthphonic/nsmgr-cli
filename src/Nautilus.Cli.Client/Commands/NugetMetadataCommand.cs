namespace Nautilus.Cli.Client.Commands;

public class NugetMetadataCommand
{
    private const string ParentElement = "PropertyGroup";
    private const string DescriptionMetadata = "Description";
    private const string AuthorsMetadata = "Authors";
    private const string OwnersMetadata = "Owners";
    private const string PackageTagsMetadata = "PackageTags";
    private const string PackageReadmeFileMetadata = "PackageReadmeFile";
    private const string CopyrightMetadata = "Copyright";
    private const string PackageVersionMetadata = "PackageVersion";
    private readonly FileInfo _projectFile;
    private readonly bool _removeMetadata;

    public NugetMetadataCommand(FileInfo projectFile, bool removeMetadata)
    {
        _projectFile = projectFile;
        _removeMetadata = removeMetadata;
    }


    public void Execute()
    {
        var prjMetadata = ProjectMetadata.SetMetadata(_projectFile.Name);
        var project = new Project(prjMetadata);
        project.Read();

        if (_removeMetadata)
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