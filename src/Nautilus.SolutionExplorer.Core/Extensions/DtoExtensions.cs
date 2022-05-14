namespace Nautilus.SolutionExplorer.Core.Extensions;

public static class DtoExtensions
{
    public static string GetCurrentVersion(this QueryPackageResponse value, string packageName)
    {
        var found = value.Data.FirstOrDefault(x => x.Id.Equals(packageName));
        if (found != null)
        {
            return found.Version;
        }
        return string.Empty;
    }
}
