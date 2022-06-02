namespace Nautilus.SolutionExplorer.Core.Abstraction;

public interface IProjectFileWriter
{
    void Initialize(ProjectMetadata projectMetadata);
    void UpdatePackageReference(string packageName, string newVersion);
    void AddOrUpdateElement(string elementName, string value);
    void AddOrUpdateElement(string parentElement, string elementName, string value);
    void DeleteElement(string parentElement, string elementName);
}