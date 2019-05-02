namespace SolutionNugetPackagesUpdater.Abstraction
{
    public interface IProjectFilePackageReader
    {
        object Read(string fileName);
    }
}