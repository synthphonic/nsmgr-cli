namespace Nautilus.SolutionExplorer.Core.FileWriters;

public class PackageConfigFileWriter : IProjectFileWriter
{
    #region IProjectFilePackageWriter implementation
    public void Initialize(ProjectMetadata projectMetadata)
    {
        // SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
    }

    public void UpdatePackageReference(string packageName, string newVersion)
    {
        // SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
    }

    public void AddOrUpdateElement(string elementName, string value)
    {
        throw new NotImplementedException();
    }

    public void AddOrUpdateElement(string parentElement, string elementName, string value)
    {
        throw new NotImplementedException();
    }

    public void DeleteElement(string parentElement, string elementName)
    {
        throw new NotImplementedException();
    }
    #endregion
}
