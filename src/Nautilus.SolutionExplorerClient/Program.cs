namespace Nautilus.SolutionExplorerClient;

class Program
{
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

                //var service = new UpdateNugetPackageService(command.SolutionFileName, command.ProjectName, command.NugetPackage, command.NugetVersion);

                try
                {
                    command.Run().Wait();
                    //service.Run().Wait();
                }
                catch (ProjectNotFoundException prjNotFoundEx)
                {
                    sw.Stop();

                    ConsoleOutputLayout.DisplayProjectNotFoundMessageFormat(prjNotFoundEx, _debugMode);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (NugetPackageNotFoundException nugetPackageNotFoundEx)
                {
                    sw.Stop();

                    ConsoleOutputLayout.DisplayNugetPackageNotFoundMessageFormat(nugetPackageNotFoundEx, _debugMode);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (CLIException cliEx)
                {
                    sw.Stop();

                    ConsoleOutputLayout.DisplayCLIExceptionMessageFormat(cliEx, _debugMode);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (SolutionFileException solutionFileEx)
                {
                    sw.Stop();

                    ConsoleOutputLayout.SolutionFileExceptionMessageFormat(solutionFileEx);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    ConsoleOutputLayout.DisplayGeneralExceptionMessageFormat(ex, _debugMode);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                        ConsoleOutputLayout.DisplayFinishingMessage(sw);
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
                    //service.Run().Wait();
                }
                catch (CLIException cliEx)
                {
                    sw.Stop();

                    ConsoleOutputLayout.DisplayCLIExceptionMessageFormat(cliEx, _debugMode);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (SolutionFileException solutionFileEx)
                {
                    sw.Stop();

                    ConsoleOutputLayout.SolutionFileExceptionMessageFormat(solutionFileEx);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (Exception ex)
                {
                    sw.Stop();

                    ConsoleOutputLayout.DisplayGeneralExceptionMessageFormat(ex, _debugMode);
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                        ConsoleOutputLayout.DisplayFinishingMessage(sw);
                    }
                }
            }))
            .WithParsed((Action<ListNugetPackagesCommand>)((command) =>
            {
                _debugMode = command.Debug;

                var sw = new Stopwatch();
                sw.Start();

                //var service = new ListNugetPackagesService(command.SolutionFileName, command.ProjectsOnly, command.UseDebugData);

                try
                {
                    command.Run().Wait();
                    //service.Run().Wait();
                }
                catch (SolutionFileException solutionFileEx)
                {
                    ConsoleOutputLayout.SolutionFileExceptionMessageFormat(solutionFileEx);

                    sw.Stop();
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (Exception ex)
                {
                    ConsoleOutputLayout.DisplayGeneralExceptionMessageFormat(ex, _debugMode);

                    sw.Stop();
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                        ConsoleOutputLayout.DisplayFinishingMessage(sw);
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
                    ConsoleOutputLayout.SolutionFileExceptionMessageFormat(solutionFileEx);

                    sw.Stop();
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                catch (Exception ex)
                {
                    ConsoleOutputLayout.DisplayGeneralExceptionMessageFormat(ex, _debugMode);

                    sw.Stop();
                    ConsoleOutputLayout.DisplayFinishingMessage(sw);
                }
                finally
                {
                    if (sw.IsRunning)
                    {
                        sw.Stop();
                        ConsoleOutputLayout.DisplayFinishingMessage(sw);
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