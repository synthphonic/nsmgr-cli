namespace NautilusCLI.Abstraction
{
    public interface IProjectFilePackageReader
    {
        object Read(string fileName);
    }
}