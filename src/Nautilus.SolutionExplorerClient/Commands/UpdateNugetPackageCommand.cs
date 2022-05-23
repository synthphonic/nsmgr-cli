/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("update-nuget-package", HelpText = "Finds the conflicting nuget package versions installed in the solution.")]
public class UpdateNugetPackageCommand : CommandBase
{
    private const string Example_Text = "Update nuget package to a specificed verison number";
    private bool _returnResults = false;

    [Option(longName: "solutionfile", shortName: 's', Required = true, HelpText = "The full file path to the .sln file.")]
    public string SolutionFileName { get; set; }

    [Option(longName: "projects-only", shortName: 'p', Required = true, HelpText = "The name of the project to update the nuget packages involved. Use 'all' to upgrade all projects")]
    public string ProjectName { get; set; }

    [Option(longName: "nuget-package", shortName: 'n', Required = true, HelpText = "The nuget package name or package identifier.")]
    public string NugetPackage { get; set; }

    [Option(longName: "nuget-version", shortName: 'g', Required = true, HelpText = "The nuget package version to upate to.")]
    public string NugetVersion { get; set; }

    [Usage(ApplicationAlias = Program.CliName)]
    public static IEnumerable<Example> Examples
    {
        get
        {
            var platformPathSample = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platformPathSample = "/users/itsme/xxx.sln";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platformPathSample = @"C:\myproject\xxx.sln";
            }

            var cmd = new UpdateNugetPackageCommand
            {
                NugetPackage = "Xamarin.Forms",
                NugetVersion = "3.6.1.21221121",
                ProjectName = "MyProject.Name",
                SolutionFileName = platformPathSample
            };

            return new List<Example>
                {
                    new Example(Example_Text, cmd)
                };
        }
    }

    public UpdateNugetPackageCommand()
    {

    }

    internal async Task Run()
    {
        var solution = ReadSolutionStructure();

        if (ProjectName.ToUpper().Equals("ALL") || ProjectName.ToLower().Equals("all"))
        {
            await UpdateAllProjects(solution);
        }
        else
        {
            await UpdatePerProject(solution);
        }
    }

    private void SetReturnResults(bool returnResults)
    {
        _returnResults = returnResults;
    }

    private async Task UpdateAllProjects(Solution solution)
    {
        _returnResults = true;
        await Run();

        //var findPackage = new FindPackageService(solution.SolutionFullPath, NugetPackage, true);
        var findPackage = new FindPackageCommand(solution.SolutionFullPath, NugetPackage, true);
        await findPackage.Run();
        var results = findPackage.Results;

        //
        // Get the intended Nuget Package information from nuget.org
        var request = NugetPackageHttpRequest.QueryRequest(NugetPackage, true);
        var result = await request.ExecuteAsync();

        var found = ValidateNugetPackage(result);

        foreach (var item in results)
        {
            var foundProject = solution.Projects.FirstOrDefault(x => x.ProjectName.Equals(item.ProjectName));
            UpdateProjectNugetPackage(foundProject);
        }

        //if (found)
        //{
        //    UpdateProjectNugetPackage(foundProject);
        //}
    }

    private async Task UpdatePerProject(Solution solution)
    {
        //
        // Validate the project existence
        //
        var foundProject = solution.Projects.FirstOrDefault(x => x.ProjectName.Equals(ProjectName));
        if (foundProject == null)
        {
            throw new ProjectNotFoundException($"Cannot find project name '{ProjectName}' in this solution");
        }

        //
        // Validate the nuget package existence
        //
        var foundNugetPackage = foundProject.Packages.FirstOrDefault(x => x.PackageName.Equals(NugetPackage));
        if (foundNugetPackage == null)
        {
            throw new NugetPackageNotFoundException($"Cannot find the nuget package '{NugetPackage}' for project '{ProjectName}'");
        }

        //
        // Get the intended Nuget Package information from nuget.org
        var request = NugetPackageHttpRequest.QueryRequest(NugetPackage, true);
        var result = await request.ExecuteAsync();

        var found = ValidateNugetPackage(result);

        if (found)
        {
            UpdateProjectNugetPackage(foundProject);
        }
    }

    private void UpdateProjectNugetPackage(Project foundProject)
    {
        var reader = new FileReader(foundProject);
        bool found = reader.TryGetPackageVersion(NugetPackage, out string packageVersion);

        if (found && !NugetVersion.Equals(packageVersion))
        {
            var writer = new FileWriter(foundProject);
            writer.UpdateNugetPackage(NugetPackage, NugetVersion);
        }
    }

    /// <summary>
    /// Search and validates a particular nuget package name and its intended version
    /// </summary>
    /// <returns><c>true</c>, if nuget package was validated, <c>false</c> otherwise.</returns>
    /// <param name="response">Response.</param>
    private bool ValidateNugetPackage(QueryPackageResponse response)
    {
        var foundPackage = response.Data.FirstOrDefault(x => x.Id.Equals(NugetPackage));
        if (foundPackage == null)
        {
            throw new NugetPackageNotFoundException($"Unable to find nuget package name {NugetPackage}");
        }

        if (foundPackage.Version.Equals(NugetVersion))
        {
            return true;
        }

        return foundPackage.Versions.Any(x => x.Version.Equals(NugetVersion));
    }

    private Solution ReadSolutionStructure()
    {
        var slnFileReader = new SolutionFileReader(SolutionFileName, true);

        try
        {
            var solution = slnFileReader.Read();
            return solution;
        }
        catch (SolutionFileException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }

    }

    internal static void Execute(UpdateNugetPackageCommand command)
    {
        bool exceptionRaised = false;
        bool debugMode = command.Debug;

        var sw = new Stopwatch();
        sw.Start();

        try
        {
            command.Run().Wait();
        }
        catch (ProjectNotFoundException prjNotFoundEx)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.DisplayProjectNotFoundMessageFormat(prjNotFoundEx, CLIConstants.LogFileName, debugMode);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        catch (NugetPackageNotFoundException nugetPackageNotFoundEx)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.DisplayNugetPackageNotFoundMessageFormat(nugetPackageNotFoundEx, CLIConstants.LogFileName, debugMode);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        catch (CLIException cliEx)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.DisplayCLIExceptionMessageFormat(cliEx, CLIConstants.LogFileName, debugMode);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        catch (SolutionFileException solutionFileEx)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.SolutionFileExceptionMessageFormat(solutionFileEx);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        catch (Exception ex)
        {
            sw.Stop();

            exceptionRaised = true;
            ConsoleMessages.DisplayGeneralExceptionMessageFormat(ex, CLIConstants.LogFileName, debugMode);
            ConsoleMessages.DisplayProgramHasTerminatedMessage();
        }
        finally
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }

            ConsoleMessages.DisplayExecutionTimeMessage(sw);

            if (!exceptionRaised)
            {
                ConsoleMessages.DisplayCompletedSuccessfullyFinishingMessage();
            }
        }
    }
}