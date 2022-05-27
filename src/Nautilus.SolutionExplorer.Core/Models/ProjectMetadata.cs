namespace Nautilus.SolutionExplorer.Core.Models;

public class ProjectMetadata
{
    public static ProjectMetadata SetMetadata(string projectFilePath)
    {
        var metadata = new ProjectMetadata
        {
            ProjectFullPath = projectFilePath,
            ProjectFileName = FileUtil.GetFileName(projectFilePath),
            ProjectType = SolutionProjectElement.CSharpProject,
            ProjectName = FileUtil.GetFileNameWithoutExtension(projectFilePath)
        };

        return metadata;
    }

    /// <summary>
    /// The project file name
    /// </summary>
    /// <value>The name of the project file.</value>
    public string ProjectFileName { get; internal set; }
    /// <summary>
    /// The full path for the solution file name
    /// </summary>
    /// <value>The name of the solution file.</value>
    public string SolutionFileName { get; internal set; }
    /// <summary>
    /// The parent path where the .sln file exists
    /// </summary>
    /// <value>The parent path.</value>
    public string ParentPath { get; internal set; }
    /// <summary>
    /// Gets or sets the project type GUID.
    /// </summary>
    /// <value>The project type GUID.</value>
    public string ProjectTypeGuid { get; internal set; }
    /// <summary>
    /// The project file name without the extension
    /// </summary>
    /// <value>The name of the project.</value>
    public string ProjectName { get; internal set; }
    /// <summary>
    /// The relative project file name from the parent path
    /// </summary>
    /// <value>The relative project path.</value>
    public string RelativeProjectPath { get; internal set; }
    /// <summary>
    /// Gets or sets the project GUID.
    /// </summary>
    /// <value>The project GUID.</value>
    public string ProjectGuid { get; internal set; }
    /// <summary>
    /// The full project file name including the path
    /// </summary>
    /// <value>The project full path.</value>
    public string ProjectFullPath { get; internal set; }
    /// <summary>
    /// The project type
    /// </summary>
    /// <value>The type of the project.</value>
    public SolutionProjectElement ProjectType { get; internal set; }
}
