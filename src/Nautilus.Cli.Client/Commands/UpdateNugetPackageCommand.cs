namespace Nautilus.Cli.Client.Commands;

public class UpdateNugetPackageCommand
{
    private const string Example_Text = "Update nuget package to a specificed verison number";
    private readonly FileInfo _solutionFile;
    private readonly string _projectName;
    private readonly string _nugetPackageName;
    private readonly string _nugetPackageVersion;
    private bool _returnResults = false;

    public UpdateNugetPackageCommand(FileInfo solutionFile, string projectName, string nugetPackageName, string nugetPackageVersion)
    {
        _solutionFile = solutionFile;
        _projectName = projectName;
        _nugetPackageName = nugetPackageName;
        _nugetPackageVersion = nugetPackageVersion;
    }

    public void Execute()
    {
        Run().Wait();
    }

    private async Task Run()
    {
        var solution = ReadSolutionStructure();

        if (_projectName.ToUpper().Equals("ALL") || _projectName.ToLower().Equals("all"))
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

        var findPackage = new FindPackageCommand(solution.SolutionFile, _nugetPackageName, true);
        await findPackage.ExecuteAsync();
        var results = findPackage.Results;

        //
        // Get the intended Nuget Package information from nuget.org
        //
        var request = NugetPackageHttpRequest.QueryRequest(_nugetPackageName, true);
        var result = await request.ExecuteAsync();

        var found = ValidateNugetPackage(result);

        foreach (var item in results!)
        {
            var foundProject = solution.Projects.FirstOrDefault(x => x.ProjectName.Equals(item.ProjectName));
            UpdateProjectNugetPackage(foundProject!);
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
        var foundProject = solution.Projects.FirstOrDefault(x => x.ProjectName.Equals(_projectName));
        if (foundProject == null)
        {
            throw new ProjectNotFoundException($"Cannot find project name '{_projectName}' in this solution");
        }

        //
        // Validate the nuget package existence
        //
        var foundNugetPackage = foundProject.Packages.FirstOrDefault(x => x.PackageName.Equals(_nugetPackageName));
        if (foundNugetPackage == null)
        {
            throw new NugetPackageNotFoundException($"Cannot find the nuget package '{_nugetPackageName}' for project '{_projectName}'");
        }

        //
        // Get the intended Nuget Package information from nuget.org
        var request = NugetPackageHttpRequest.QueryRequest(_nugetPackageName, true);
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
        bool found = reader.TryGetPackageVersion(_nugetPackageName, out string packageVersion);

        if (found && !_nugetPackageVersion.Equals(packageVersion))
        {
            var writer = new FileWriterContext(foundProject);
            writer.UpdateNugetPackage(_nugetPackageName, _nugetPackageVersion);
        }
    }

    /// <summary>
    /// Search and validates a particular nuget package name and its intended version
    /// </summary>
    /// <returns><c>true</c>, if nuget package was validated, <c>false</c> otherwise.</returns>
    /// <param name="response">Response.</param>
    private bool ValidateNugetPackage(QueryPackageResponse response)
    {
        var foundPackage = response.Data.FirstOrDefault(x => x.Id.Equals(_nugetPackageName));
        if (foundPackage == null)
        {
            throw new NugetPackageNotFoundException($"Unable to find nuget package name {_nugetPackageName}");
        }

        if (foundPackage.Version.Equals(_nugetPackageVersion))
        {
            return true;
        }

        return foundPackage.Versions.Any(x => x.Version.Equals(_nugetPackageVersion));
    }

    private Solution ReadSolutionStructure()
    {
        var slnFileReader = new SolutionFileReader(_solutionFile, true);

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