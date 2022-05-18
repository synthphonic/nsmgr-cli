namespace Nautilus.SolutionExplorer.Core.Cache;

public class NugetPackageCache
{
    private static readonly Lazy<NugetPackageCache> _cacheInstance = new Lazy<NugetPackageCache>(() => new NugetPackageCache());
    private readonly Dictionary<string, LatestPackageInfo> _packageInfoDictionary = new Dictionary<string, LatestPackageInfo>();
    private readonly Dictionary<string, string> _notFoundPackageInfoDictionary = new Dictionary<string, string>();

    private NugetPackageCache()
    {

    }

    public static NugetPackageCache Instance
    {
        get { return _cacheInstance.Value; }
    }

    public void AddNotFoundPackage(string packageName, string version)
    {
        if (!_notFoundPackageInfoDictionary.ContainsKey(packageName))
        {
            _notFoundPackageInfoDictionary[packageName] = version;
        }
    }

    public void AddPackage(string packageName, string version, Datum selectedDatum)
    {
        if (!_packageInfoDictionary.ContainsKey(packageName))
        {
            _packageInfoDictionary[packageName] = new LatestPackageInfo(packageName, version, selectedDatum);
        }
    }

    internal LatestPackageInfo GetPackage(string packageName)
    {
        var found = _packageInfoDictionary.TryGetValue(packageName, out LatestPackageInfo value);

        return found ? value : null;
    }

    public bool ContainsPackage(string packageName)
    {
        var exists = _packageInfoDictionary.ContainsKey(packageName);
        return exists;
    }
}

internal record LatestPackageInfo(string PackageName, string Version, Datum Data);