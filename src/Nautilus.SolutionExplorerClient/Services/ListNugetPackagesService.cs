namespace Nautilus.SolutionExplorerClient.Services;

public class ListNugetPackagesService
{
    private string _solutionFileName;
    private readonly bool _processProjectsOnly;

    public ListNugetPackagesService(string solutionFileName, bool processProjectsOnly = false, bool debugData = false)
    {
        _processProjectsOnly = processProjectsOnly;
        _solutionFileName = solutionFileName;
        TestDataHelper.UseTestData = debugData;
    }

    public async Task Run()
    {
        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        var slnFileReader = new SolutionFileReader(_solutionFileName, _processProjectsOnly);
        var solution = slnFileReader.Read();

        var flatList = solution.ExtractNugetPackageAsFlatList();
        var categorizedByPackageNameList = solution.CategorizeByPackageName(flatList);

        var packageNames = new List<string>();
        foreach (var item in categorizedByPackageNameList)
        {
            packageNames.Add(item.Key);
        }

        var latestPackages = await QueryNugetPackageOnlineAsync(packageNames.ToArray(), CliStringFormatter.WriteOnlinePackageProgressHandler);

        Colorful.Console.WriteLine();

        categorizedByPackageNameList = categorizedByPackageNameList.OrderBy(x => x.Key).ToDictionary();

        WriteOutput(categorizedByPackageNameList, solution.SolutionFileName, solution.Projects.Count(), latestPackages);
    }

    private void WriteOutput(Dictionary<string, IList<NugetPackageReferenceExtended>> nugetPackages, string solutionFileName, int totalProjects, Dictionary<string, string> latestPackages)
    {
        Colorful.Console.WriteLine();
        Colorful.Console.Write("{0,-15}", "Solution ");
        Colorful.Console.WriteLine($": {solutionFileName}", Color.PapayaWhip);
        Colorful.Console.Write("Total Projects : ");
        Colorful.Console.WriteLine($"{totalProjects}", Color.PapayaWhip);

        Colorful.Console.WriteLine();
        Colorful.Console.Write("Found ", Color.PapayaWhip);
        Colorful.Console.Write($"{nugetPackages.Count()} ", Color.Aqua);
        Colorful.Console.WriteLine("Nuget Packages...", Color.PapayaWhip);
        Colorful.Console.WriteLine();

        if (!nugetPackages.Any())
        {
            Colorful.Console.WriteLine("** Great! No conflict found for this solution **", Color.DeepSkyBlue);
        }

        foreach (var package in nugetPackages)
        {
            Colorful.Console.WriteLine($"{package.Key}", Color.Aqua);

            var sortedByProjects = package.Value.OrderBy(x => x.ProjectName).ToList();

            foreach (var item in sortedByProjects)
            {
                Colorful.Console.Write($"In Project ");
                Colorful.Console.Write(CliStringFormatter.Format40, Color.Azure, item.ProjectName);
                Colorful.Console.Write("[{0,-16}]", Color.Azure, item.ProjectTargetFramework);
                Colorful.Console.Write(" found version ");
                Colorful.Console.Write("{0}", Color.Azure, item.Version);
                Colorful.Console.WriteLine();
            }

            var latestVersion = latestPackages[package.Key];
            if (latestVersion.Contains("Unable"))
            {
                Colorful.Console.Write("Latest nuget package online : ", Color.Goldenrod);
                Colorful.Console.WriteLine("{0}", Color.Red, latestVersion);
            }
            else
            {
                Colorful.Console.WriteLine("*Latest nuget package online : {0}", Color.Goldenrod, latestVersion);
            }

            Colorful.Console.WriteLine();
        }
    }

    private async Task<Dictionary<string, string>> QueryNugetPackageOnlineAsync(string[] packageNameList, Action writeProgressHandler = null)
    {
        var result = new Dictionary<string, string>();

        foreach (var packageName in packageNameList)
        {
            var request = NugetPackageHttpRequest.QueryRequest(packageName, false);
            var response = await request.ExecuteAsync();

            if (response.Exception != null)
            {
                result[packageName] = "Unable to connect to the internet";
            }
            else
            {
                var packageVersion = response.GetCurrentVersion(packageName);
                result[packageName] = packageVersion;
            }

            writeProgressHandler?.Invoke();
        }

        return result;
    }
}
