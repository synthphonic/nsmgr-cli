namespace Nautilus.SolutionExplorer.Core.Configurations;

public static class TargetFrameworkSetting
{
    private static Dictionary<string, ProjectTargetFramework> _versions;

    static TargetFrameworkSetting()
    {
        _versions = new Dictionary<string, ProjectTargetFramework>
        {
            ["netstandard2.0"] = ProjectTargetFramework.NETStandard20,
            ["netcoreapp2.2"] = ProjectTargetFramework.NETCoreApp22,
            ["netcoreapp2.1"] = ProjectTargetFramework.NETCoreApp21,
            ["netcoreapp2.0"] = ProjectTargetFramework.NETCoreApp20,
            ["net5.0"] = ProjectTargetFramework.NET5,
            ["net6.0"] = ProjectTargetFramework.NET6,
        };
    }

    /// <summary>
    /// Get the specified innerText.
    /// <para/>
    /// Sample string is as follows
    /// <para/>
    /// netstandard2.0, netcoreapp2.2, netcoreapp2.1, netcoreapp2.0
    /// </summary>
    /// <returns>The get.</returns>
    /// <param name="stringValue">Inner text.</param>
    internal static ProjectTargetFramework Get(string stringValue)
    {
        try
        {
            var found = _versions.FirstOrDefault(x => x.Key.Equals(stringValue));
        }
        catch (ArgumentNullException)
        {
            return ProjectTargetFramework.Unknown;
        }

        return _versions[stringValue];
    }
}
