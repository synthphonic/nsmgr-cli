/*
 * ref: https://stackoverflow.com/questions/14110212/reading-specific-xml-elements-from-xml-file
 */

namespace Nautilus.SolutionExplorer.Core;

public class FileReader
{
    private readonly Dictionary<ProjectTargetFramework, IProjectFilePackageReader> _fileReaders;
    private readonly ProjectMetadata _metadata;
    private readonly ProjectTargetFramework _targetFramework;
    private readonly bool _packagesConfigFileExist;

    public FileReader(Project project) : this()
    {
        _metadata = project.Metadata;
        _targetFramework = project.TargetFramework;
        _packagesConfigFileExist = project.PackagesConfigFileExist;
    }

    internal FileReader(ProjectTargetFramework targetFramework, ProjectMetadata projectMetadata) : this()
    {
        _metadata = projectMetadata;
        _targetFramework = targetFramework;
    }

    private FileReader()
    {
        _fileReaders = new Dictionary<ProjectTargetFramework, IProjectFilePackageReader>
        {
            //[SolutionProjectElement.PackageConfig] = new PackageConfigFileReader(),
            //[SolutionProjectElement.iOS] = new CSharpNativeProjectFileReader(),
            //[SolutionProjectElement.Android] = new CSharpNativeProjectFileReader()
            [ProjectTargetFramework.NETFramework] = new CSharpNETFrameworkProjectFileReader(),
            [ProjectTargetFramework.NETFramework20] = new PackageConfigFileReader(),
            [ProjectTargetFramework.NETFramework35] = new PackageConfigFileReader(),
            [ProjectTargetFramework.NETFramework40] = new PackageConfigFileReader(),
            [ProjectTargetFramework.NETFramework45] = new PackageConfigFileReader(),
            [ProjectTargetFramework.NETFramework46] = new PackageConfigFileReader(),
            [ProjectTargetFramework.NETFramework47] = new PackageConfigFileReader(),
            [ProjectTargetFramework.NETFramework48] = new PackageConfigFileReader(),
            [ProjectTargetFramework.NETStandard20] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NETStandard21] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NETCoreApp20] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NETCoreApp21] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NETCoreApp22] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NETCoreApp30] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NETCoreApp31] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NET5] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NET6] = new CSharpProjectFileReader(),
            [ProjectTargetFramework.NativeiOS] = new CSharpNETFrameworkProjectFileReader(),
            [ProjectTargetFramework.NativeiOSBinding] = new CSharpNETFrameworkProjectFileReader(),
            [ProjectTargetFramework.NativeAndroid] = new CSharpNETFrameworkProjectFileReader(),
            [ProjectTargetFramework.NativeUWP] = new CSharpNETFrameworkProjectFileReader()
        };
    }

    public object ReadFile()
    {
        //
        // SOME NOTES:
        // for ProjectTarget.NETFramework46, we need to check the following:
        // 1. check if packages.json file exists. if yes, then read from it
        // 2. if packages.json does not exists, then read from C# project file
        //

        try
        {
            var packageConfigExists = false;

            object returnedObject = null;

            if (_targetFramework == ProjectTargetFramework.NETFramework20 ||
                _targetFramework == ProjectTargetFramework.NETFramework35 ||
                _targetFramework == ProjectTargetFramework.NETFramework40 ||
                _targetFramework == ProjectTargetFramework.NETFramework45 ||
                _targetFramework == ProjectTargetFramework.NETFramework46 ||
                _targetFramework == ProjectTargetFramework.NETFramework47 ||
                _targetFramework == ProjectTargetFramework.NETFramework48)
            {
                var projectFileName = _metadata.ProjectFullPath;
                var packageConfigFile = Path.Combine(Path.GetDirectoryName(projectFileName), "packages.config");
                packageConfigExists = File.Exists(packageConfigFile);

                returnedObject = packageConfigExists
                    ? _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath)
                    : _fileReaders[ProjectTargetFramework.NETFramework].Read(_metadata.ProjectFullPath);
            }
            else
            {
                returnedObject = _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath);
            }

            return returnedObject;
        }
        catch (KeyNotFoundException keyNotFoundEx)
        {
            var exceptionMessage = new StringBuilder();
            exceptionMessage.AppendFormat($"File cannot be read for '{_metadata.ProjectFileName}'\n");
            exceptionMessage.AppendFormat($"Reason:{keyNotFoundEx.Message}");
            throw new CLIException(exceptionMessage.ToString(), keyNotFoundEx);

        }
        catch (FileNotFoundException fileNotFoundEx)
        {
            var exceptionMessage = new StringBuilder();
            exceptionMessage.AppendFormat($"This error shouldn't have happened! Something went seriously wrong at '{_metadata.ProjectFileName}'\n");
            exceptionMessage.AppendFormat($"Reason:{fileNotFoundEx.Message}");
            throw new CLIException(exceptionMessage.ToString(), fileNotFoundEx);
        }
        catch (Exception ex)
        {
            var exceptionMessage = new StringBuilder();
            exceptionMessage.AppendFormat($"This error shouldn't have happened! Something went seriously wrong at '{_metadata.ProjectFileName}'\n");
            exceptionMessage.AppendFormat($"Reason:{ex.Message}");
            throw new CLIException(exceptionMessage.ToString(), ex);
        }
    }

    public bool TryGetPackageVersion(string packageName, out string version)
    {
        version = string.Empty;

        try
        {
            IList<NugetPackageReference> packageReferences = null;
            NugetPackageReference found = null;

            if (_targetFramework == ProjectTargetFramework.NETFramework20 ||
                _targetFramework == ProjectTargetFramework.NETFramework35 ||
                _targetFramework == ProjectTargetFramework.NETFramework40 ||
                _targetFramework == ProjectTargetFramework.NETFramework45 ||
                _targetFramework == ProjectTargetFramework.NETFramework46 ||
                _targetFramework == ProjectTargetFramework.NETFramework47 ||
                _targetFramework == ProjectTargetFramework.NETFramework48)
            {
                if (_packagesConfigFileExist)
                {
                    packageReferences = _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath) as IList<NugetPackageReference>;
                    found = packageReferences.FirstOrDefault(x => x.PackageName.Equals(packageName));
                    version = found.Version;

                    return !string.IsNullOrWhiteSpace(found.Version);
                }

                packageReferences = _fileReaders[ProjectTargetFramework.NETFramework].Read(_metadata.ProjectFullPath) as IList<NugetPackageReference>;
                found = packageReferences.FirstOrDefault(x => x.PackageName.Equals(packageName));
                version = found.Version;

                return !string.IsNullOrWhiteSpace(found.Version);
            }

            packageReferences = _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath) as IList<NugetPackageReference>;
            found = packageReferences.FirstOrDefault(x => x.PackageName.Equals(packageName));
            version = found.Version;

            return !string.IsNullOrWhiteSpace(found.Version);
        }
        catch (InvalidOperationException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
