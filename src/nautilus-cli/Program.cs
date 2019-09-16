using System;
using System.Diagnostics;
using CommandLine;
using Nautilus.Cli.Client.CommandLine.Services;
using Nautilus.Cli.Client.CommandLine.Verbs;
using Nautilus.Cli.Client.OutputMessages;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.TestData;

namespace Nautilus.Cli.Client
{
	class Program
	{
        public const string Name = "nautilus-cli";
		private const string LogFileName = "nautilus-cli.log";
		private static bool _exceptionRaised;

		static void Main(string[] args)
		{
            bool _debugMode = false;

#if DEBUG
			TestDataHelper.UseTestData = false;
#endif

			_ = Parser.Default.ParseArguments<FindPackage, ListProjects, ListNugetPackages, UpdateNugetPackage>(args)
				.WithParsed((Action<UpdateNugetPackage>)((command) =>
				{
                    _debugMode = command.Debug;

                    var sw = new Stopwatch();
					sw.Start();

					var service = new UpdateNugetPackageService(command.SolutionFileName, command.ProjectName, command.NugetPackage, command.NugetVersion);

					try
					{
						service.Run().Wait();
					}
					catch(ProjectNotFoundException prjNotFoundEx)
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
				.WithParsed((Action<ListProjects>)((command) =>
				{
                    _debugMode = command.Debug;

					var sw = new Stopwatch();
					sw.Start();

					var service = new ListProjectsService(command.SolutionFileName, command.ProjectsOnly, command.ShowNugetPackages, command.NugetPackageUpdates);

					try
					{
						service.Run().Wait();
					}
					catch(CLIException cliEx)
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
				.WithParsed((Action<ListNugetPackages>)((command) =>
				{
                    _debugMode = command.Debug;

                    var sw = new Stopwatch();
					sw.Start();

					var service = new ListNugetPackagesService(command.SolutionFileName, command.ProjectsOnly, command.UseDebugData);

					try
					{
						service.Run().Wait();
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
                .WithParsed((Action<FindPackage>)((command) =>
                {
                    _debugMode = command.Debug;

                    var sw = new Stopwatch();
                    sw.Start();

					var service = new FindPackageService(command.SolutionFileName, command.NugetPackage);

                    try
                    {
                        service.Run().Wait();
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
					//
					// TODO: what approach should we take here?
					//
				});
		}
	}
}