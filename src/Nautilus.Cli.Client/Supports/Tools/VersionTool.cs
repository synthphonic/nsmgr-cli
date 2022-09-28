namespace Nautilus.Cli.Client.Supports.Tools;

internal static class VersionTool
{
    public static FileVersionInfo GetVersion()
    {
        var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location!);
        return version;
    }
}