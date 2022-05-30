namespace Nautilus.SolutionExplorerClient;

class Program
{
    static void Main(string[] args)
    {
        string[] internalArgs;
        if (args.Length == 0)
        {
            internalArgs = "--help".Split();
        }
        else
        {
            internalArgs = args;
        }

        CommandlineStartup(internalArgs);
    }

    static void CommandlineStartup(string[] args)
    {

#if DEBUG
        TestDataHelper.UseTestData = false;
#endif

        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<FindPackageCommand, ListProjectsCommand, ListNugetPackagesCommand, UpdateNugetPackageCommand, ModifyProjectVersionCommand>(args);
        parserResult
            .WithParsed<UpdateNugetPackageCommand>(options => UpdateNugetPackageCommand.Execute(options))
            .WithParsed<ListProjectsCommand>(options => ListProjectsCommand.Execute(options))
            .WithParsed<ListNugetPackagesCommand>(options => ListNugetPackagesCommand.Execute(options))
            .WithParsed<FindPackageCommand>(options => FindPackageCommand.Execute(options))
            .WithParsed<ModifyProjectVersionCommand>(options => ModifyProjectVersionCommand.Execute(options))
            .WithNotParsed(errs => DisplayHelp(parserResult, errs));
    }

    private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errors)
    {
        var helpText = HelpText.AutoBuild(result, h =>
        {
            var assemblyTitle = (AssemblyTitleAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false).FirstOrDefault();
            var assemblyProduct = (AssemblyProductAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false).FirstOrDefault();
            var assemblyDescription = (AssemblyDescriptionAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).FirstOrDefault();
            var assemblyCopyright = (AssemblyCopyrightAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false).FirstOrDefault();
            var assemblyInfoVersion = (AssemblyInformationalVersionAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault();

            h.AdditionalNewLineAfterOption = false;
            h.Heading = $"{assemblyTitle.Title} v{assemblyInfoVersion.InformationalVersion}\n{assemblyProduct.Product}";
            h.Copyright = $"{assemblyCopyright.Copyright}";
            h.AddDashesToOption = true;
            h.AddNewLineBetweenHelpSections = true;
            h.AdditionalNewLineAfterOption = true;

            return HelpText.DefaultParsingErrorsHandler(result, h);

        }, e => e, verbsIndex: true, maxDisplayWidth: 150);

        Console.WriteLine(helpText);
    }

    internal static void DisplayProductInfo()
    {
        var assemblyTitle = (AssemblyTitleAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false).FirstOrDefault();
        var assemblyProduct = (AssemblyProductAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false).FirstOrDefault();
        var assemblyDescription = (AssemblyDescriptionAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).FirstOrDefault();
        var assemblyCopyright = (AssemblyCopyrightAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false).FirstOrDefault();
        var assemblyInfoVersion = (AssemblyInformationalVersionAttribute)typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault();

        Console.WriteLine($"{assemblyTitle.Title} v{assemblyInfoVersion.InformationalVersion}");
        Console.WriteLine($"{assemblyProduct.Product}");
        Console.WriteLine($"{assemblyCopyright.Copyright}");
    }
}