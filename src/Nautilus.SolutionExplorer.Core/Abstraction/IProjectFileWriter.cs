namespace Nautilus.SolutionExplorer.Core.Abstraction;

public interface IProjectFileWriter
{
    void Initialize(ProjectMetadata projectMetadata);
    void UpdatePackageReference(string packageName, string newVersion);
    void AddOrUpdateElement(string elementName, string value);
}