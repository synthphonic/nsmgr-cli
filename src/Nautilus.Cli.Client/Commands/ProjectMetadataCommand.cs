namespace Nautilus.Cli.Client.Commands;

/// <summary>
/// Project metadata command
/// </summary>
internal sealed class ProjectMetadataCommand : CommandBase
{
    private readonly string _metadata;
    private readonly FileInfo _projectFile;
    private readonly bool _removeMetadata;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="metadata"></param>
    /// <param name="projectFile"></param>
    /// <param name="removeMetadata">set to true to delete a metadata, else it is either add new or update an existing metadata</param>
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