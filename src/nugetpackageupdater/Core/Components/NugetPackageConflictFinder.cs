using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using SolutionNugetPackagesUpdater.Core.Models;

namespace SolutionNugetPackagesUpdater.Core.Components
{
	public class NugetPackageConflictFinder : IApplicationComponent
	{
		private object[] _parameters;

		public void Initialize(params object[] paramteters)
		{
			_parameters = paramteters;
		}

		public object Execute()
		{
			var solution = _parameters[0] as Solution;
			var flatList = ExtractAsFlatList(solution);

			var duplicateList = FindConflictPackages(flatList);

			return duplicateList;
		}

		private static List<NugetPackageReferenceExtended> ExtractAsFlatList(Solution solution)
		{
			var flatNugetPackageList = new List<NugetPackageReferenceExtended>();

			foreach (var project in solution.Projects)
			{
				var packages = project.Packages.ToList();
				foreach (var package in packages)
				{
					var extended = new NugetPackageReferenceExtended(project, package);
					flatNugetPackageList.Add(extended);
				}
			}

			return flatNugetPackageList;
		}

		private Dictionary<string, IList<NugetPackageReferenceExtended>> FindConflictPackages(List<NugetPackageReferenceExtended> flatList)
		{
			#region To test same package but different version
			//var debugFound = flatList.FirstOrDefault(x => x.PackageName.Contains("Xamarin.Forms"));
			//var packageRef = new NugetPackageReference(debugFound.PackageName, "4.3.1.221222");
			//var newDebug = new NugetPackageReferenceExtended("Storiveo.ShaZee.Core", packageRef);
			//flatList.Add(newDebug);
			#endregion

			var distincts = flatList.DistinctBy(x => x.PackageVersionName).OrderBy(x => x.PackageVersionName).ToList();

			var distinctArray = distincts.ToArray();
			var newDuplicateList = new Dictionary<string, IList<NugetPackageReferenceExtended>>();

			foreach (var item in distinctArray)
			{
				List<NugetPackageReferenceExtended> duplicateList = null;

				var foundDuplicates = (from x in distincts
									   where
										   x.PackageName.Equals(item.PackageName)
									   select x).ToList().Count > 1;


				if (foundDuplicates)
				{
					duplicateList = distincts.Where(x => x.PackageName.Equals(item.PackageName)).ToList();

					if (!newDuplicateList.ContainsKey(item.PackageName))
					{
						newDuplicateList[item.PackageName] = duplicateList;
					}
				}
			}

			return newDuplicateList;
		}
	}
}