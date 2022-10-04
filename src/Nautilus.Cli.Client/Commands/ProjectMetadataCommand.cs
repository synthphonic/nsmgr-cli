namespace Nautilus.Cli.Client.Commands;

internal sealed class ProjectMetadataCommand : CommandBase
{
    private readonly string _metadata;
    private readonly FileInfo _projectFile;
    private readonly bool _removeMetadata;

    public ProjectMetadataCommand(string metadata, FileInfo projectFile, bool removeMetadata)
    {
        _metadata = metadata;
        _projectFile = projectFile;
        _removeMetadata = removeMetadata;
    }

    public override async Task ExecuteAsync()
    {
        //
        // project-property -p ../../../../../../nautilus/Nautilus.IO.csproj -t PropertyGroup:Authors=aa,bb,cc
        //

        // Parse the element name and value
        var elementInfo = XmlParser.ParseFromCliParameter(_metadata);

        var prjMetadata = ProjectMetadata.SetMetadata(_projectFile.FullName);
        var project = new Project(prjMetadata);
        project.Read();

        if (_removeMetadata)
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

        await Task.CompletedTask;
    }
}