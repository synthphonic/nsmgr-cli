namespace Nautilus.SolutionExplorer.Core.FileReaders;

public class CSharpNETCoreProjectFileReader : IProjectFileReader
{
    public IEnumerable<NugetPackageReference> ReadNugetPackages(string fileName)
    {
        var xmlContent = FileUtil.ReadFileContent(fileName);

        var xElement = XElement.Parse(xmlContent);

        var itemGroups = xElement.Elements("ItemGroup").ToList();
        var packageReferences = itemGroups.Elements("PackageReference").ToList();

        var packageReferenceList = new List<NugetPackageReference>();
        foreach (var item in packageReferences)
        {
            var include = item.FirstAttribute.Value;
            var version = item.FirstAttribute.NextAttribute.Value;

            var packageRefItem = new NugetPackageReference(include, version);
            packageReferenceList.Add(packageRefItem);
        }

        return packageReferenceList;
    }

    public string ReadVersion(string fileName)
    {
        var xmlContent = FileUtil.ReadFileContent(fileName);

        var xElement = XElement.Parse(xmlContent);

        var versionElement = xElement.Descendants("Version").FirstOrDefault();

        //
        // another way of getting the Version string value
        //
        //var itemGroups = xElement.Elements("PropertyGroup").ToList();
        //var versionElement = itemGroups.Elements("Version").ToList();

        return versionElement != null ? versionElement.Value : null;            
    }
}
