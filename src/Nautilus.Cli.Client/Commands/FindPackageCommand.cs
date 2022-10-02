namespace Nautilus.Cli.Client.Commands;

internal sealed class FindPackageCommand
{
    private readonly bool _returnResults;

    internal FindPackageCommand(FileInfo solutionFile, string nugetPackage, bool returnResults = false)
    {
        SolutionFile = solutionFile;
        NugetPackage = nugetPackage;
        _returnResults = returnResults;
    }

    public void Execute()
    {
        Run().Wait();
    }

    private async Task Run()
    {

        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        var slnFileReader = new SolutionFileReader(SolutionFile, true);
        var solution = slnFileReader.Read();

        var flatList = solution.ExtractNugetPackageAsFlatList();
        var categorizedByPackageNameList = solution.CategorizeByPackageName(flatList);

        Colorful.Console.WriteLine();

        if (categorizedByPackageNameList.ContainsKey(NugetPackage))
        {
            var projects = categorizedByPackageNameList[NugetPackage];

            if (_returnResults)
            {
                Results = projects;
            }
            else
            {
                WriteOutput(projects, solution.SolutionFile);
            }

            return;
        }

        Colorful.Console.WriteLine($"'{NugetPackage}' nuget package is not referenced in any project.\n");

        await Task.Delay(1);
    }

    private void WriteOutput(IList<NugetPackageReferenceExtended> projects, FileInfo solutionFile)
    {
        Colorful.Console.WriteLine();
        Colorful.Console.Write("{0,-15}", "Solution ");
        Colorful.Console.WriteLine($": {solutionFile}", Color.PapayaWhip);

        Colorful.Console.WriteLine();

        Colorful.Console.WriteLine($"{NugetPackage}", Color.Aqua);

        foreach (var project in projects)
        {
            Colorful.Console.Write($"In Project ");
            Colorful.Console.Write(CliStringFormatter.Format40, Color.Azure, project.ProjectName);
            Colorful.Console.Write("[{0,-16}]", Color.Azure, project.ProjectTargetFramework);
            Colorful.Console.Write(" found version ");
            Colorful.Console.Write("{0}", Color.Azure, project.Version);
            Colorful.Console.WriteLine();
        }
    }

    public IList<NugetPackageReferenceExtended>? Results { get; private set; }
    public string? NugetPackage { get; }
    public FileInfo? SolutionFile { get; }
}