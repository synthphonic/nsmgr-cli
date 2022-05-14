namespace Nautilus.SolutionExplorer.Core.Components.Http;

public static class NugetPackageHttpRequest
{
    public static NugetPackageQuery QueryRequest(string packageName, bool preRelease)
    {
        return new NugetPackageQuery(packageName, preRelease);
    }
}