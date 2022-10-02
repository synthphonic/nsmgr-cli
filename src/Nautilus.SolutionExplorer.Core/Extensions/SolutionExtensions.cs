namespace Nautilus.SolutionExplorer.Core.Extensions;

public static class SolutionExtensions
{
    public static List<NugetPackageReferenceExtended> ExtractNugetPackageAsFlatList(this Solution solution)
    {
        var flatNugetPackageList = new List<NugetPackageReferenceExtended>();

        foreach (var project in solution.Projects)
        {
            if (project.Packages is not null)
            {
                var packages = project.Packages.ToList();
                foreach (var package in packages)
                {
                    var extended = new NugetPackageReferenceExtended(project, package);
                    flatNugetPackageList.Add(extended);
                }
            }
        }

        return flatNugetPackageList;
    }

    public static Dictionary<string, IList<NugetPackageReferenceExtended>> CategorizeByPackageName(this Solution solution, List<NugetPackageReferenceExtended> flatList)
    {
        var categorizedByPackage = new Dictionary<string, IList<NugetPackageReferenceExtended>>();
        foreach (var item in flatList)
        {
            if (!categorizedByPackage.ContainsKey(item.PackageName))
            {
                categorizedByPackage[item.PackageName] = new List<NugetPackageReferenceExtended>();
                categorizedByPackage[item.PackageName].Add(item);
            }
            else
            {
                categorizedByPackage[item.PackageName].Add(item);
            }
        }

        return categorizedByPackage;
    }
}
