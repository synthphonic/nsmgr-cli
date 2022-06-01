namespace Nautilus.SolutionExplorerClient;

class Program
{
    private static Stopwatch _sw = null;
    private static bool _exceptionRaised = false;

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

        var commands = new List<Type>
        {
            typeof(UpdateNugetPackageCommand),
            typeof(ListProjectsCommand),
            typeof(ListNugetPackagesCommand),
            typeof(ListProjectsTargetFrameworkCommand),
            typeof(FindPackageCommand),
            typeof(ModifyProjectVersionCommand)
        };

        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments(args, commands.ToArray());
        parserResult
            .WithParsed((Action<CommandBase>)(cmd =>
            {
                ExecuteCommand(cmd);
            }))
            .WithNotParsed(errs => DisplayHelp(parserResult, errs));
    }

    private static void ExecuteCommand(CommandBase cmd)
    {
        _sw = new Stopwatch();
        _sw.Start();

        try
        {
            cmd.Execute();
            _sw.Stop();

            Console.WriteLine();
            //ConsoleOutputLayout.DisplayExecutionTimeMessage(_sw);
            //Console.WriteLine($"Completed in {_sw.Elapsed}");
            Console.WriteLine();
        }
        catch (ProjectNotFoundException prjNotFoundEx)
        {
            _exceptionRaised = true;
            ConsoleOutputLayout.DisplayProjectNotFoundMessageFormat(prjNotFoundEx, CLIConstants.LogFileName, cmd.Debug);
            ConsoleOutputLayout.DisplayProgramHasTerminatedMessage();
        }
        catch (NugetPackageNotFoundException nugetPackageNotFoundEx)
        {
            _exceptionRaised = true;
            ConsoleOutputLayout.DisplayNugetPackageNotFoundMessageFormat(nugetPackageNotFoundEx, CLIConstants.LogFileName, cmd.Debug);
            ConsoleOutputLayout.DisplayProgramHasTerminatedMessage();
        }
        catch (CLIException cliEx)
        {
            _exceptionRaised = true;
            ConsoleOutputLayout.DisplayCLIExceptionMessageFormat(cliEx, CLIConstants.LogFileName, cmd.Debug);
            ConsoleOutputLayout.DisplayProgramHasTerminatedMessage();
        }
        catch (SolutionFileException solutionFileEx)
        {
            _exceptionRaised = true;
            ConsoleOutputLayout.SolutionFileExceptionMessageFormat(solutionFileEx);
            ConsoleOutputLayout.DisplayProgramHasTerminatedMessage();
        }
        catch (Exception ex)
        {
            _exceptionRaised = true;
            ConsoleOutputLayout.DisplayGeneralExceptionMessageFormat(ex, CLIConstants.LogFileName, cmd.Debug);
            ConsoleOutputLayout.DisplayProgramHasTerminatedMessage();
        }
        finally
        {
            if (_sw != null && _sw.IsRunning)
            {
                _sw.Stop();
            }

            ConsoleOutputLayout.DisplayExecutionTimeMessage(_sw);

            if (!_exceptionRaised)
            {
                ConsoleOutputLayout.DisplayCompletedSuccessfullyFinishingMessage();
            }
        }
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