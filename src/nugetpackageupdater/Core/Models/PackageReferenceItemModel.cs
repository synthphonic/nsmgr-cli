namespace SolutionNugetPackagesUpdater.Core.Models
{
    public class PackageReferenceItemModel
    {
        public PackageReferenceItemModel(string include, string version)
        {
            Include = include;
            Version = version;
        }

        public string Include { get; private set; }
        public string Version { get; private set; }
    }
}
