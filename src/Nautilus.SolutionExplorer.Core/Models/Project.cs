namespace Nautilus.SolutionExplorer.Core.Models;

public class Project
{
    public Project(ProjectMetadata metadata)
    {
        Metadata = metadata;

        CheckPackagesConfigFileExistence();

        if (TestDataHelper.UseTestData)
        {
            Metadata.ProjectName = Metadata.ProjectName.Replace("Storiveo", "FourtyNineLabs");
            Metadata.ProjectName = Metadata.ProjectName.Replace("Niu", "FourtyNineLabs");
        }

        if (Metadata.ProjectType == SolutionProjectElement.CSharpProject)
        {
            var projectTypeManager = new ProjectTypeManager(Metadata.ProjectFullPath);
            TargetFramework = projectTypeManager.GetTargetFramework();
            ProjectType = Metadata.ProjectType;

            var fileFinder = new FileReader(TargetFramework, Metadata);
            var fileContentObject = fileFinder.ReadFile();
            Packages = fileContentObject as IEnumerable<NugetPackageReference>;
        }
        else if (Metadata.ProjectType == SolutionProjectElement.VirtualFolder)
        {
            TargetFramework = ProjectTarget.Unknown;
        }

        ProjectType = Metadata.ProjectType;
    }

    private void CheckPackagesConfigFileExistence()
    {
        var packageConfigFile = Path.Combine(Metadata.ProjectFullPath, "packages.config");
        PackagesConfigFileExist = File.Exists(packageConfigFile);
    }

    /// <summary>
    /// Gets the project GUID.
    /// </summary>
    /// <value>The project GUID.</value>
    public string ProjectGuid { get { return Metadata.ProjectGuid; } }
    /// <summary>
    /// Gets the name of the project.
    /// </summary>
    /// <value>The name of the project.</value>
    public string ProjectName { get { return Metadata.ProjectName; } }
    /// <summary>
    /// Gets the target framework.
    /// </summary>
    /// <value>The target framework.</value>
    public ProjectTarget TargetFramework { get; private set; }
    /// <summary>
    /// Gets the packages.
    /// </summary>
    /// <value>The packages.</value>
    public IEnumerable<NugetPackageReference> Packages { get; private set; }
    /// <summary>
    /// Gets the type of the project.
    /// </summary>
    /// <value>The type of the project.</value>
    public SolutionProjectElement ProjectType { get; private set; }
    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    public ProjectMetadata Metadata { get; private set; }
    /// <summary>
    /// Returns true if the packages.config file exists or not for a project
    /// </summary>
    /// <value><c>true</c> if packages config file exists; otherwise, <c>false</c>.</value>
    public bool PackagesConfigFileExist { get; private set; }
}
