namespace Nautilus.SolutionExplorer.Core;

public class FileWriterContext
{
    private readonly Dictionary<ProjectTargetFramework, IProjectFileWriter> _fileWriters;
    private readonly Project _project;

    private FileWriterContext()
    {
        _fileWriters = new Dictionary<ProjectTargetFramework, IProjectFileWriter>
        {
            [ProjectTargetFramework.NETCoreApp20] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NETCoreApp21] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NETCoreApp22] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NETCoreApp30] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NETCoreApp31] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NETStandard20] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NETStandard21] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NET5] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NET6] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NETStandard21] = new CSharpNETCoreProjectFileWriter(),
            [ProjectTargetFramework.NativeAndroid] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NativeiOS] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NativeUWP] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NETFramework] = new CSharpNETFrameworkProjectFileWriter(),
            [ProjectTargetFramework.NETFramework46] = new PackageConfigFileWriter()
        };
    }

    public FileWriterContext(Project project) : this()
    {
        _project = project;
    }

    public bool UpdateNugetPackage(string packageName, string version)
    {
        IProjectFileWriter fileWriter;
        if (_project.TargetFramework == ProjectTargetFramework.NETFramework20 ||
            _project.TargetFramework == ProjectTargetFramework.NETFramework35 ||
            _project.TargetFramework == ProjectTargetFramework.NETFramework40 ||
            _project.TargetFramework == ProjectTargetFramework.NETFramework45 ||
            _project.TargetFramework == ProjectTargetFramework.NETFramework46 ||
            _project.TargetFramework == ProjectTargetFramework.NETFramework47 ||
            _project.TargetFramework == ProjectTargetFramework.NETFramework48)
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

        fileWriter.Initialize(_project.Metadata);
        fileWriter.UpdatePackageReference(packageName, version);

        return true;
    }

    public void UpdateElement(string elementName, string value)
    {
        var fileWriter = _fileWriters[_project.TargetFramework];
        fileWriter.Initialize(_project.Metadata);
        fileWriter.AddOrUpdateElement(elementName, value);
    }

    public void AddOrUpdateElement(string parentElement, string elementName, string value)
    {
        var fileWriter = _fileWriters[_project.TargetFramework];
        fileWriter.Initialize(_project.Metadata);
        fileWriter.AddOrUpdateElement(parentElement, elementName, value);
    }

    public void DeleteElement(string parentElement, string elementName)
    {
        var fileWriter = _fileWriters[_project.TargetFramework];
        fileWriter.Initialize(_project.Metadata);
        fileWriter.DeleteElement(parentElement, elementName);
    }
}
