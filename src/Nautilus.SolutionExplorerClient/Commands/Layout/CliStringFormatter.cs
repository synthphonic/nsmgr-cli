namespace Nautilus.SolutionExplorerClient.Commands.Layout;

internal static class CliStringFormatter
{
    internal const string Format1 = "{0,-1}";
    internal const string Format5 = "{0,-5}";
    internal const string Format7 = "{0,-7}";
    internal const string Format9 = "{0,-9}";
    internal const string Format15 = "{0,-15}";
    internal const string Format30 = "{0,-30}";
    internal const string Format40 = "{0,-40}";
    internal const string Format45 = "{0,-45}";
    internal const string Format50 = "{0,-50}";
    internal const string Format54 = "{0,-54}";

    internal static void WriteOnlinePackageProgressHandler()
    {
        Task.Run(() => Colorful.Console.Write(".", Color.DeepSkyBlue));
    }
}
