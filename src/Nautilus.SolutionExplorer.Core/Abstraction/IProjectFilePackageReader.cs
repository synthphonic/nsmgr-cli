namespace Nautilus.SolutionExplorer.Core.Abstraction
{
    public interface IProjectFilePackageReader
    {
        object Read(string fileName);
    }
}