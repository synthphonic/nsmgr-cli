namespace Nautilus.SolutionExplorer.Core;

public class FileWriter
{
    private readonly Dictionary<ProjectTargetFramework, IProjectFilePackageWriter> _fileWriters;
    private readonly Project _project;

    private FileWriter()
    {
        _fileWriters = new Dictionary<ProjectTargetFramework, IProjectFilePackageWriter>
        {
            [ProjectTargetFramework.NETCoreApp22] = new CSharpNETStandardProjectFileWriter(),
            [ProjectTargetFramework.NETStandard20] = new CSharpNETStandardProjectFileWriter(),
            [ProjectTargetFramework.NativeAndroid] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NativeiOS] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NativeUWP] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NETFramework] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NETFramework46] = new PackageConfigFileWriter()
        };
    }

    public FileWriter(Project project) : this()
    {
        _project = project;
    }

    public bool UpdateNugetPackage(string packageName, string version)
    {
        IProjectFilePackageWriter fileWriter = null;

        if (_project.TargetFramework == ProjectTargetFramework.NETFramework46)
        {
            if (_project.PackagesConfigFileExist)
            {
                fileWriter = _fileWriters[_project.TargetFramework];
            }
            else
            {
                fileWriter = _fileWriters[ProjectTargetFramework.NETFramework];
            }
        }
        else
        {
            fileWriter = _fileWriters[_project.TargetFramework];
        }

        fileWriter.Initialize(_project.TargetFramework, _project.Metadata);
        fileWriter.UpdatePackageReference(packageName, version);

        return true;
    }
}
