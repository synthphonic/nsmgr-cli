namespace Nautilus.SolutionExplorer.Core.Models;

//public class Test
//{
//    public void TestMethod()
//    {
//        var project = new Project(null);
//        project
//            .ParentElement("parentElement")
//            .Create("PackageVersion", "1.0.0-preview3");

//    }
//}

public class Project
{
    private Action _restoreAction = null;
    private Action _backupAction = null;
    private Action _readAction = null;

    public Project(ProjectMetadata metadata)
    {
        Metadata = metadata;

        ProjectFullPath = metadata.ProjectFullPath;

        _restoreAction = Restore;
        _backupAction = Backup;
        _readAction = ReadbyProjectMetadata;
    }

    public Project(FileInfo projectFile)
    {
        ProjectFile = projectFile;
        ProjectFullPath = projectFile.FullName;

        _readAction = ReadByProjectFile;
    }

    public IProjectElementValue ParentElement(string parentElement)
    {
        var projectProperty = new ProjectProperty(this);
        return projectProperty.SetParent(parentElement);
    }

    public void Update(string elementName, string value)
    {
        var fileWriter = new FileWriterContext(this);
        fileWriter.UpdateElement(elementName, value);
    }

    public void Restore()
    {
        if (_restoreAction is not null)
            _restoreAction();
    }

    public void Backup()
    {
        if (_backupAction is not null)
            BackupFile.Execute(Metadata.ProjectFullPath);
    }

    public void Read()
    {
        if (_readAction is not null)
            _readAction();
    }

    private void ReadByProjectFile()
    {
        CheckPackagesConfigFileExistence();

        var projectTypeManager = new ProjectTargetFrameworkExtractor(ProjectFile.FullName);
        TargetFramework = projectTypeManager.GetTargetFramework();
        ProjectType = SolutionProjectElement.CSharpProject;

        //var fileReader = new FileReaderContext(TargetFramework, Metadata);
        var fileReader = new FileReaderContext(TargetFramework, ProjectFile.FullName);
        Packages = fileReader.ReadNugetPackages();
        Version = fileReader.ReadVersion();
        ProjectName = ProjectFile.Name;
    }

    private void ReadbyProjectMetadata()
    {
        CheckPackagesConfigFileExistence();

        //if (TestDataHelper.UseTestData)
        //{
        //    Metadata.ProjectName = Metadata.ProjectName.Replace("Storiveo", "FourtyNineLabs");
        //    Metadata.ProjectName = Metadata.ProjectName.Replace("Niu", "FourtyNineLabs");
        //}

        if (Metadata.ProjectType == SolutionProjectElement.CSharpProject)
        {
            var projectTypeManager = new ProjectTargetFrameworkExtractor(ProjectFullPath);
            TargetFramework = projectTypeManager.GetTargetFramework();
            ProjectType = Metadata.ProjectType;
            ProjectName = Metadata.ProjectName;

            var fileReader = new FileReaderContext(TargetFramework, Metadata);
            Packages = fileReader.ReadNugetPackages();
            Version = fileReader.ReadVersion();
        }
        else if (Metadata.ProjectType == SolutionProjectElement.VirtualFolder)
        {
            TargetFramework = ProjectTargetFramework.Unknown;
        }

        ProjectType = Metadata.ProjectType;
    }

    private void CheckPackagesConfigFileExistence()
    {
        var packageConfigFile = Path.Combine(ProjectFullPath, "packages.config");
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
    public string ProjectName { get; private set; }
    //public string ProjectName { get { return Metadata.ProjectName; } }

    /// <summary>
    /// Gets the target framework.
    /// </summary>
    /// <value>The target framework.</value>
    public ProjectTargetFramework TargetFramework { get; private set; }

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
    /// Gets the profile file information
    /// </summary>
    public FileInfo ProjectFile { get; private set; }

    /// <summary>
    /// Gets the project full path
    /// </summary>
    public string ProjectFullPath { get; private set; }

    /// <summary>
    /// Returns true if the packages.config file exists or not for a project
    /// </summary>
    /// <value><c>true</c> if packages config file exists; otherwise, <c>false</c>.</value>
    public bool PackagesConfigFileExist { get; private set; }

    /// <summary>
    /// The project version. Element: "Version"
    /// </summary>
    public string Version { get; private set; }
}