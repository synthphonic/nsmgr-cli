using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NautilusCLI.Abstraction;
using NautilusCLI.Core.Models;

namespace NautilusCLI.Core.FileReaders
{
    public class CSharpProjectFileReader : IProjectFilePackageReader
    {
        private string _fileName;

		public object Read(string fileName)
        {
			_fileName = fileName;
            return ReadCsharpProjectFile();
        }

        private IList<NugetPackageReference> ReadCsharpProjectFile()
        {
            var fileName = Path.GetFileName(_fileName);
            List<NugetPackageReference> packageReferenceList = null;

            var xmlContent = string.Empty;
            using (var fs = File.OpenRead(_fileName))
            {
                using (var sr = new StreamReader(fs))
                {
                    xmlContent = sr.ReadToEnd();
                }
            }

            var xElement = XElement.Parse(xmlContent);

            var itemGroups = xElement.Elements("ItemGroup").ToList();
            var packageReferences = itemGroups.Elements("PackageReference").ToList();

            packageReferenceList = new List<NugetPackageReference>();
            foreach (var item in packageReferences)
            {
                var include = item.FirstAttribute.Value;
                var version = item.FirstAttribute.NextAttribute.Value;

                var packageRefItem = new NugetPackageReference(include, version);
                packageReferenceList.Add(packageRefItem);
            }

            return packageReferenceList;
        }
    }
}
