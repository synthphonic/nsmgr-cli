namespace Nautilus.SolutionExplorerClient.Core;

public abstract class CommandBase
{
    [Option("debug", Default = false, Required = false, Hidden = true, HelpText = "Show debugging message including exception message and stacktrace")]
    public bool Debug { get; set; }
}