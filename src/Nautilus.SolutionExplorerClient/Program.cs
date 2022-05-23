namespace Nautilus.SolutionExplorerClient;

class Program
{
    internal const string CliName = $"nsmgr";

    static void Main(string[] args)
    {

#if DEBUG
        TestDataHelper.UseTestData = false;
#endif

        _ = Parser.Default.ParseArguments<FindPackageCommand, ListProjectsCommand, ListNugetPackagesCommand, UpdateNugetPackageCommand>(args)
            .WithParsed((Action<UpdateNugetPackageCommand>)((command) => UpdateNugetPackageCommand.Execute(command)))
            .WithParsed((Action<ListProjectsCommand>)((command) => ListProjectsCommand.Execute(command)))
            .WithParsed((Action<ListNugetPackagesCommand>)((command) => ListNugetPackagesCommand.Execute(command)))
            .WithParsed((Action<FindPackageCommand>)((command) => FindPackageCommand.Execute(command)))
            .WithNotParsed(errs =>
            {
                //var sb = new StringBuilder();
                //foreach (var item in errs)
                //{
                //	sb.AppendFormat($"{item.ToString()}");
                //}

                //Console.WriteLine(sb.ToString());
            });
    }
}