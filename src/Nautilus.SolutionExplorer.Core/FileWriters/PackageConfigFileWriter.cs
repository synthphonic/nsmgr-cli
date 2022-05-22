namespace Nautilus.SolutionExplorer.Core.FileWriters;

public class PackageConfigFileWriter : IProjectFilePackageWriter
{
    #region IProjectFilePackageWriter implementation
    public void Initialize(ProjectTargetFramework targetFramework, ProjectMetadata projectMetadata)
    {
        // SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
    }

    public void UpdatePackageReference(string packageName, string newVersion)
    {
        // SHAH: TO IMPLEMENT IProjectFilePackageWriter interface
    }
    #endregion
}
