using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.Utils;

namespace Nautilus.Cli.Core.FileReaders
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
			var xmlContent = FileUtil.ReadFileContent(_fileName);

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
    }
}
