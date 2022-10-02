namespace Nautilus.SolutionExplorer.Core.Abstraction;

public abstract class CommandBase
{
    public CommandBase(bool showFullError = false)
    {
        ShowFullError = showFullError;
    }

    public abstract Task ExecuteAsync();
    public bool ShowFullError { get; set; }
}