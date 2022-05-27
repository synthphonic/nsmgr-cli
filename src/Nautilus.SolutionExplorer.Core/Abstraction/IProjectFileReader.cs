namespace Nautilus.SolutionExplorer.Core.Abstraction
{
    /// <summary>
    /// The csproj file reader interface
    /// </summary>
    public interface IProjectFileReader
    {
        /// <summary>
        /// Read the nuget packages in the csproj file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IEnumerable<NugetPackageReference> ReadNugetPackages(string fileName);

        /// <summary>
        /// Read the version string from the Version element in the csproj file. If none exists, it will return null
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Returns the version string number, if non exists return null.</returns>
        string ReadVersion(string fileName);
    }
}