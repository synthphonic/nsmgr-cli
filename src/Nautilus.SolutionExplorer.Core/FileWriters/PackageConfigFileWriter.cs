namespace Nautilus.SolutionExplorer.Core.FileWriters;

public class PackageConfigFileWriter : IProjectFileWriter
{
    #region IProjectFilePackageWriter implementation
    public void Initialize(ProjectMetadata projectMetadata)
    {
        // SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
    }

    public void AddOrUpdateElement(string elementName, string value)
    {
        throw new NotImplementedException();
    }

    public void UpdatePackageReference(string packageName, string newVersion)
    {
        // SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
    }
    #endregion
}
