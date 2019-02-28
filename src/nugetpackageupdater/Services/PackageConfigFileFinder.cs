using System.Collections.Generic;
using System.Linq;
using SolutionNugetPackagesUpdater.Models;

namespace SolutionNugetPackagesUpdater.Services
{
	public class PackageConfigFileFinder
	{
		private readonly PackageConfig _packageConfig;

		public PackageConfigFileFinder(PackageConfig packageConfig)
		{
			_packageConfig = packageConfig;
		}

		public IEnumerable<Package> FindAllId(string id)
		{
			var results = _packageConfig.Packages.Where(x => x.Id.Equals(id)).ToList();
			return results;
		}

        public IEnumerable<Package> FindAll()
        {
            return _packageConfig.Packages;
        }
    }
}