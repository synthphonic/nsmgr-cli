namespace Nautilus.Cli.Core.Abstraction
{
    public interface IProjectFilePackageReader
    {
        object Read(string fileName);
    }
}