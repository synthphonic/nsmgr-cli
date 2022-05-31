/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("update-nuget-package", HelpText = Example_Text)]
public class UpdateNugetPackageCommand : CommandBase
{
    private const string Example_Text = "Update nuget package to a specificed verison number";
    private bool _returnResults = false;

    #region Command Line Setup & Options
    [Option(longName: "solutionfile", shortName: 's', Required = true, HelpText = "The full file path to the .sln file.")]
    public string SolutionFileName { get; set; }

    [Option(longName: "projects-only", shortName: 'p', Required = true, HelpText = "The name of the project to update the nuget packages involved. Use 'all' to upgrade all projects")]
    public string ProjectName { get; set; }

    [Option(longName: "nuget-package", shortName: 'n', Required = true, HelpText = "The nuget package name or package identifier.")]
    public string NugetPackage { get; set; }

    [Option(longName: "nuget-version", shortName: 'g', Required = true, HelpText = "The nuget package version to upate to.")]
    public string NugetVersion { get; set; }

    [Usage()]
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
    #endregion

    public UpdateNugetPackageCommand()
    {

    }

    public override void Execute()
    {
        Run().Wait();
    }

    private async Task Run()
    {
        Program.DisplayProductInfo();

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

    private async Task UpdateAllProjects(Solution solution)
    {
        _returnResults = true;

        await Run();

        var findPackage = new FindPackageCommand(solution.SolutionFullPath, NugetPackage, true);
        findPackage.Execute();
        var results = findPackage.Results;

        //
        // Get the intended Nuget Package information from nuget.org
        //
        var request = NugetPackageHttpRequest.QueryRequest(NugetPackage, true);
        var result = await request.ExecuteAsync();

        var found = ValidateNugetPackage(result);

        foreach (var item in results)
        {
            var foundProject = solution.Projects.FirstOrDefault(x => x.ProjectName.Equals(item.ProjectName));
            UpdateProjectNugetPackage(foundProject);
        }
    }

    private void SetReturnResults(bool returnResults)
    {
        _returnResults = returnResults;
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
        var reader = new FileReaderContext(foundProject);
        bool found = reader.TryGetPackageVersion(NugetPackage, out string packageVersion);

        if (found && !NugetVersion.Equals(packageVersion))
        {
            var writer = new FileWriterContext(foundProject);
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
}