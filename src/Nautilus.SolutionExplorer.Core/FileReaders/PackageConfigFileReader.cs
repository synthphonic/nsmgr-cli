namespace Nautilus.SolutionExplorer.Core.FileReaders;

public class PackageConfigFileReader : IProjectFileReader
{
    private string _fileName;
    private string _packageConfigFile;

    public IEnumerable<NugetPackageReference> ReadNugetPackages(string fileName)
    {
        _fileName = fileName;
        _packageConfigFile = Path.Combine(Path.GetDirectoryName(_fileName), "packages.config");

        var packageConfig = ReadPackageConfig();

        var nugetPackageRefs = (from item in packageConfig.Packages
                                select new NugetPackageReference(item.Id, item.Version, item.TargetFramework)).ToList();

        return nugetPackageRefs;
    }

    public string ReadVersion(string fileName)
    {
        throw new NotImplementedException();
    }

    private PackageConfig ReadPackageConfig()
    {
        var serializer = new XmlSerializer(typeof(PackageConfig));
        using (var sr = new StreamReader(_packageConfigFile))
        {
            return (PackageConfig)serializer.Deserialize(sr);
        }
    }
}
