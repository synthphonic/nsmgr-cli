namespace Nautilus.SolutionExplorerClient;

class Program
{
    private const string LogFileName = "nsmgr-cli.log";
    private static bool _exceptionRaised;
    internal const string CliName = $"nsmgr";

    static void Main(string[] args)
    {
        bool _debugMode = false;

#if DEBUG
        TestDataHelper.UseTestData = false;
#endif

        _ = Parser.Default.ParseArguments<FindPackageCommand, ListProjectsCommand, ListNugetPackagesCommand, UpdateNugetPackageCommand>(args)
            .WithParsed((Action<UpdateNugetPackageCommand>)((command) =>
            {
                _debugMode = command.Debug;

                var sw = new Stopwatch();
                sw.Start();

                try
                {
                    command.Run().Wait();
                }
                catch (ProjectNotFoundException prjNotFoundEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayProjectNotFoundMessageFormat(prjNotFoundEx, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                catch (NugetPackageNotFoundException nugetPackageNotFoundEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayNugetPackageNotFoundMessageFormat(nugetPackageNotFoundEx, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                catch (CLIException cliEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayCLIExceptionMessageFormat(cliEx, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                catch (SolutionFileException solutionFileEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.SolutionFileExceptionMessageFormat(solutionFileEx);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayGeneralExceptionMessageFormat(ex, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                    }

                    ConsoleMessages.DisplayExecutionTimeMessage(sw);

                    if (!_exceptionRaised)
                    {
                        ConsoleMessages.DisplayCompletedSuccessfullyFinishingMessage();
                    }
                }
            }))
            .WithParsed((Action<ListProjectsCommand>)((command) =>
            {
                _debugMode = command.Debug;

                var sw = new Stopwatch();
                sw.Start();

                try
                {
                    command.Run().Wait();
                }
                catch (CLIException cliEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayCLIExceptionMessageFormat(cliEx, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                catch (SolutionFileException solutionFileEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.SolutionFileExceptionMessageFormat(solutionFileEx);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayGeneralExceptionMessageFormat(ex, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                    }

                    ConsoleMessages.DisplayExecutionTimeMessage(sw);

                    if (!_exceptionRaised)
                    {
                        ConsoleMessages.DisplayCompletedSuccessfullyFinishingMessage();
                    }
                }
            }))
            .WithParsed((Action<ListNugetPackagesCommand>)((command) =>
            {
                _debugMode = command.Debug;

                var sw = new Stopwatch();
                sw.Start();

                try
                {
                    command.Run().Wait();
                }
                catch (SolutionFileException solutionFileEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.SolutionFileExceptionMessageFormat(solutionFileEx);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();

                }
                catch (Exception ex)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayGeneralExceptionMessageFormat(ex, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                    }

                    ConsoleMessages.DisplayExecutionTimeMessage(sw);

                    if (!_exceptionRaised)
                    {
                        ConsoleMessages.DisplayCompletedSuccessfullyFinishingMessage();
                    }
                }
            }))
            .WithParsed((Action<FindPackageCommand>)((command) =>
            {
                _debugMode = command.Debug;

                var sw = new Stopwatch();
                sw.Start();

                try
                {
                    command.Run().Wait();
                }
                catch (SolutionFileException solutionFileEx)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.SolutionFileExceptionMessageFormat(solutionFileEx);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    _exceptionRaised = true;
                    ConsoleMessages.DisplayGeneralExceptionMessageFormat(ex, LogFileName, _debugMode);
                    ConsoleMessages.DisplayProgramHasTerminatedMessage();
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                    }

                    ConsoleMessages.DisplayExecutionTimeMessage(sw);

                    if (!_exceptionRaised)
                    {
                        ConsoleMessages.DisplayCompletedSuccessfullyFinishingMessage();
                    }
                }
            }))
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