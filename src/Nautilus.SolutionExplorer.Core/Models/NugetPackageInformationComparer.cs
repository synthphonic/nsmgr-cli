namespace Nautilus.SolutionExplorer.Core.Models;

public class NugetPackageInformationComparer
{
    private readonly NugetPackageReference _localPackage;
    private readonly Datum _onlinePackage;

    public NugetPackageInformationComparer(NugetPackageReference localPackage, Datum onlinePackageDatum)
    {
        _localPackage = localPackage;
        _onlinePackage = onlinePackageDatum;

        localPackage.Version.Equals(onlinePackageDatum);
    }

    public string PackageName { get { return _localPackage.PackageName; } }

    public bool IsLatestVersion { get { return _localPackage.Version.Equals(_onlinePackage.Version); } }

    public string OnlineVersion { get { return _onlinePackage.Version; } }

    public string LocalVersion { get { return _localPackage.Version; } }

    public bool OnlinePackageExists { get { return _onlinePackage != null; } }
}