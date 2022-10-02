namespace Nautilus.Cli.Client.Commands;

internal sealed class ListNugetPackagesCommand : CommandBase
{
    private readonly FileInfo _solutionFile;
    private readonly bool _projectsOnly;

    public ListNugetPackagesCommand(FileInfo solutionFile, bool projectsOnly, bool showFullError = false) : base(showFullError)
    {
        _solutionFile = solutionFile;
        _projectsOnly = projectsOnly;
    }

    public override async Task ExecuteAsync()
    {
        await RunAsync();
    }

    private async Task RunAsync()
    {
        if (!_solutionFile.Extension.Contains("sln"))
        {
            throw new CommandException($"Unrecognize file type ({_solutionFile.Extension})");
        }

        var slnFileReader = new SolutionFileReader(_solutionFile, _projectsOnly);
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

        WriteOutput(categorizedByPackageNameList, solution.SolutionFile, solution.Projects.Count(), latestPackages);
    }

    private void WriteOutput(Dictionary<string, IList<NugetPackageReferenceExtended>> nugetPackages, FileInfo solutionFile, int totalProjects, Dictionary<string, string> latestPackages)
    {
        Colorful.Console.WriteLine();
        Colorful.Console.Write("{0,-15}", "Solution ");
        Colorful.Console.WriteLine($": {solutionFile.Name}", Color.PapayaWhip);
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
                Colorful.Console.Write($"Project : ");
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

    private async Task<Dictionary<string, string>> QueryNugetPackageOnlineAsync(string[] packageNameList, Action writeProgressHandler = null!)
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
