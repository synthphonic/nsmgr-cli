using Nautilus.SolutionExplorer.Core.Cache;

namespace Nautilus.SolutionExplorer.Core.Components;

public class NugetPackageOnlineComparer
{
    private readonly Solution _solution;
    private readonly bool _showPreReleaseNugetPackages;
    private readonly Action _writeProgressHandler;

    public NugetPackageOnlineComparer(Solution solution, bool showPreReleaseNugetPackages, Action writeProgressHandler = null)
    {
        _solution = solution;
        _showPreReleaseNugetPackages = showPreReleaseNugetPackages;
        _writeProgressHandler = writeProgressHandler;
        Result = new Dictionary<string, IList<NugetPackageInformationComparer>>();
    }

    public async Task Run()
    {
        foreach (var project in _solution.Projects)
        {
            await GetOnlinePackagesAsync(project);
        }
    }

    private async Task GetOnlinePackagesAsync(Project project)
    {
        var projectPackages = new List<NugetPackageInformationComparer>();

        if (project.ProjectType == SolutionProjectElement.CSharpProject)
        {
            foreach (var package in project.Packages)
            {
                if (NugetPackageCache.Instance.ContainsPackage(package.PackageName))
                {
                    var latestPackageInfo = NugetPackageCache.Instance.GetPackage(package.PackageName);
                    projectPackages.Add(new NugetPackageInformationComparer(package, latestPackageInfo.Data));

                    continue;
                }

                var requestor = NugetPackageHttpRequest.QueryRequest(package.PackageName, _showPreReleaseNugetPackages);
                var response = await requestor.ExecuteAsync();
                var selectedDatum = response.Data.FirstOrDefault(x => x.Id.Equals(package.PackageName));

                // if selectedDatum is null, try do ToLower() on the Id property.
                if (selectedDatum == null)
                {
                    selectedDatum = response.Data.FirstOrDefault(x => x.Id.ToLower().Equals(package.PackageName));
                }

                if (selectedDatum != null)
                {
                    NugetPackageCache.Instance.AddPackage(selectedDatum.Id, selectedDatum.Version, selectedDatum);
                }
                else
                {
                    NugetPackageCache.Instance.AddNotFoundPackage(package.PackageName, package.Version);
                }

                projectPackages.Add(new NugetPackageInformationComparer(package, selectedDatum));

                _writeProgressHandler?.Invoke();
            }
        }

        Result[project.ProjectName] = projectPackages;
    }

    public Dictionary<string, IList<NugetPackageInformationComparer>> Result { get; private set; }
}
