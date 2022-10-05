namespace Nautilus.SolutionExplorer.Core.FileReaders;

public class CSharpNETFrameworkProjectFileReader : IProjectFileReader
{
    public IEnumerable<NugetPackageReference> ReadNugetPackages(string fileName)
    {
        var nugetPackages = new List<NugetPackageReference>();

        var xmlContent = FileUtil.ReadFileContent(fileName);

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlContent);

        var nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsMgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

        foreach (XmlNode xmlNode in xmlDoc.SelectNodes("//x:PackageReference", nsMgr))
        {
            var packageName = xmlNode.Attributes["Include"].Value;
            var packageVersion = xmlNode.InnerText;

            //
            // SHAH: NOTE on PackageReference "Version" data in .csproj file
            /* We need this since there were some modification to the .csproj file structure 
             * Old strucutre:
             * <PackageReference Include="Xamarin.Forms">
             * 	<Version>4.0.0.425677</Version>
             * </PackageReference>	
             * 
             * New structure:
             * <PackageReference Include="Xamarin.Forms" Version="4.0.0.425677" />  
             */

            if (string.IsNullOrWhiteSpace(packageVersion))
            {
                packageVersion = xmlNode.Attributes["Version"].Value;
            }

            var package = new NugetPackageReference(packageName, packageVersion);

            nugetPackages.Add(package);
        }

        return nugetPackages;
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