using System.Collections.Generic;
using System.IO;
using System.Xml;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Models;

namespace Nautilus.Cli.Core.FileReaders
{
	public class CSharpNativeProjectFileReader : IProjectFilePackageReader
    {
        private string _fileName;

        public object Read(string fileName)
        {
            _fileName = fileName;

            return ReadCsharpProjectFile();
        }

        private IList<NugetPackageReference> ReadCsharpProjectFile()
        {
			var nugetPackages = new List<NugetPackageReference>();

            var xmlContent = string.Empty;
            using (var fs = File.OpenRead(_fileName))
            {
                using (var sr = new StreamReader(fs))
                {
                    xmlContent = sr.ReadToEnd();
                }
            }

			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlContent);

			var xmlNsManager = new XmlNamespaceManager(xmlDoc.NameTable);
			xmlNsManager.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

			foreach (XmlNode xmlNode in xmlDoc.SelectNodes("//x:PackageReference", xmlNsManager))
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
