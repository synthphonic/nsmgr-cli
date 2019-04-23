using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SolutionNugetPackagesUpdater.Abstraction;
using SolutionNugetPackagesUpdater.Core.Models;

namespace SolutionNugetPackagesUpdater.Core.FileReaders
{
    public class CSharpProjectFileReader : IFileReader
    {
        private string _file;

        public object Read(string file)
        {
            _file = file;

            return ReadCsharpProjectFile();
        }

        private IList<PackageReferenceItemModel> ReadCsharpProjectFile()
        {
            var fileName = Path.GetFileName(_file);
            List<PackageReferenceItemModel> packageReferenceList = null;

            var xmlContent = string.Empty;
            using (var fs = File.OpenRead(_file))
            {
                using (var sr = new StreamReader(fs))
                {
                    xmlContent = sr.ReadToEnd();
                }
            }

            var xElement = XElement.Parse(xmlContent);

            var itemGroups = xElement.Elements("ItemGroup").ToList();
            var packageReferences = itemGroups.Elements("PackageReference").ToList();

            packageReferenceList = new List<PackageReferenceItemModel>();
            foreach (var item in packageReferences)
            {
                var include = item.FirstAttribute.Value;
                var version = item.FirstAttribute.NextAttribute.Value;

                var packageRefItem = new PackageReferenceItemModel(include, version);
                packageReferenceList.Add(packageRefItem);
            }

            return packageReferenceList;
        }
    }
}
