namespace Nautilus.SolutionExplorer.Core.Models;

internal class ProjectTypeInfo
{
    public ProjectTypeInfo(SolutionProjectElement solutionProjectElement, string guid)
    {
        SolutionProjectElement = solutionProjectElement;
        Guid = guid;
    }

    public SolutionProjectElement SolutionProjectElement { get; private set; }
    public string Guid { get; private set; }
}
