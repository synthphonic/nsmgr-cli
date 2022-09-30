namespace Nautilus.Cli.Client.Commands;

internal sealed class ProjectMetadataCommand
{
    private readonly string _metadata;
    private readonly string _projectFile;
    private readonly bool _removeMetadata;

    public ProjectMetadataCommand(string metadata, string projectFile, bool removeMetadata)
    {
        _metadata = metadata;
        _projectFile = projectFile;
        _removeMetadata = removeMetadata;
    }

    public void Execute()
    {
        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        //
        // project-property -p ../../../../../../nautilus/Nautilus.IO.csproj -t PropertyGroup:Authors=aa,bb,cc
        //

        // Parse the element name and value
        var elementInfo = XmlParser.ParseFromCliParameter(_metadata);

        var prjMetadata = ProjectMetadata.SetMetadata(_projectFile);
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
    }
}