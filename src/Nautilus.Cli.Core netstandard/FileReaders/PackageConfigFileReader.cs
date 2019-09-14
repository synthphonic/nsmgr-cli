using System.IO;
using System.Xml.Serialization;
using Nautilus.Cli.Core.Models;
using System.Linq;
using Nautilus.Cli.Core.Abstraction;

namespace Nautilus.Cli.Core.FileReaders
{
	public class PackageConfigFileReader : IProjectFilePackageReader
    {
        private string _fileName;
		private string _packageConfigFile;

		public object Read(string fileName)
		{
			_fileName = fileName;
			_packageConfigFile = Path.Combine(Path.GetDirectoryName(_fileName), "packages.config");

			var packageConfig = ReadPackageConfig();

			var nugetPackageRefs = (from item in packageConfig.Packages
									select new NugetPackageReference(item.Id, item.Version, item.TargetFramework)).ToList();

			return nugetPackageRefs;
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
}
