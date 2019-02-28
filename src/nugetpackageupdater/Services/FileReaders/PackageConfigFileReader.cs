using System.IO;
using System.Xml.Serialization;
using SolutionNugetPackagesUpdater.Abstraction;
using SolutionNugetPackagesUpdater.Models;

namespace SolutionNugetPackagesUpdater.Services.FileReaders
{
    public class PackageConfigFileReader : IFileReader
    {
        private string _file;

        public object Read(string file)
        {
            _file = file;

            return ReadPackageConfig();
        }

        private PackageConfig ReadPackageConfig()
        {
            var serializer = new XmlSerializer(typeof(PackageConfig));
            using (var sr = new StreamReader(_file))
            {
                return (PackageConfig)serializer.Deserialize(sr);
            }
        }
    }
}
