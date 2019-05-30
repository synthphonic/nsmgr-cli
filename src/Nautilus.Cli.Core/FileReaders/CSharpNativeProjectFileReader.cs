using System.Collections.Generic;
using System.Xml;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.Utils;

namespace Nautilus.Cli.Core.FileReaders
{
	public class CSharpNativeProjectFileReader : IProjectFilePackageReader
    {
        private string _fileName;

        public object Read(string fileName)
        {
            _fileName = fileName;

            return ReadCSharpProjectFile();
        }

        private IList<NugetPackageReference> ReadCSharpProjectFile()
        {
			var nugetPackages = new List<NugetPackageReference>();

			var xmlContent = FileUtil.ReadFileContent(_fileName);

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlContent);

			var nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsMgr.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

			foreach (XmlNode xmlNode in xmlDoc.SelectNodes("//x:PackageReference", nsMgr))
			{
				var packageName = xmlNode.Attributes["Include"].Value;
				var packageVersion = xmlNode.InnerText;
				var package = new NugetPackageReference(packageName, packageVersion);

				nugetPackages.Add(package);
			}

			return nugetPackages;
        }
    }
}
