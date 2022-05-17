//namespace Nautilus.SolutionExplorerClient.Services;

//public class UpdateNugetPackageService
//{
//    private readonly string _solutionFileName;
//    private readonly string _projectName;
//    private readonly string _packageName;
//    private readonly string _version;

//    public UpdateNugetPackageService(string solutionFileName, string projectName, string packageName, string version)
//    {
//        _solutionFileName = solutionFileName;
//        _projectName = projectName;
//        _packageName = packageName;
//        _version = version;
//    }

//    internal async Task Run()
//    {
//        var solution = ReadSolutionStructure();

//        if (_projectName.ToUpper().Equals("ALL") || _projectName.ToLower().Equals("all"))
//        {
//            await UpdateAllProjects(solution);
//        }
//        else
//        {
//            await UpdatePerProject(solution);
//        }
//    }

//    private async Task UpdateAllProjects(Solution solution)
//    {
//        var findPackage = new FindPackageService(solution.SolutionFullPath, _packageName, true);
//        await findPackage.Run();
//        var results = findPackage.Results;

//        //
//        // Get the intended Nuget Package information from nuget.org
//        //var request = NugetPackageHttpRequest.QueryRequest(_packageName, true);
//        //var result = await request.ExecuteAsync();

//        //var found = ValidateNugetPackage(result);

//        foreach (var item in results)
//        {
//            var foundProject = solution.Projects.FirstOrDefault(x => x.ProjectName.Equals(item.ProjectName));
//            UpdateProjectNugetPackage(foundProject);
//        }

//        //if (found)
//        //{
//        //    UpdateProjectNugetPackage(foundProject);
//        //}
//    }

//    private async Task UpdatePerProject(Solution solution)
//    {
//        //
//        // Validate the project existence
//        //
//        var foundProject = solution.Projects.FirstOrDefault(x => x.ProjectName.Equals(_projectName));
//        if (foundProject == null)
//        {
//            throw new ProjectNotFoundException($"Cannot find project name '{_projectName}' in this solution");
//        }

//        //
//        // Validate the nuget package existence
//        //
//        var foundNugetPackage = foundProject.Packages.FirstOrDefault(x => x.PackageName.Equals(_packageName));
//        if (foundNugetPackage == null)
//        {
//            throw new NugetPackageNotFoundException($"Cannot find the nuget package '{_packageName}' for project '{_projectName}'");
//        }

//        //
//        // Get the intended Nuget Package information from nuget.org
//        var request = NugetPackageHttpRequest.QueryRequest(_packageName, true);
//        var result = await request.ExecuteAsync();

//        var found = ValidateNugetPackage(result);

//        if (found)
//        {
//            UpdateProjectNugetPackage(foundProject);
//        }
//    }

//    private void UpdateProjectNugetPackage(Project foundProject)
//    {
//        var reader = new FileReader(foundProject);
//        bool found = reader.TryGetPackageVersion(_packageName, out string packageVersion);

//        if (found && !_version.Equals(packageVersion))
//        {
//            var writer = new FileWriter(foundProject);
//            writer.UpdateNugetPackage(_packageName, _version);
//        }
//    }

//    /// <summary>
//    /// Search and validates a particular nuget package name and its intended version
//    /// </summary>
//    /// <returns><c>true</c>, if nuget package was validated, <c>false</c> otherwise.</returns>
//    /// <param name="response">Response.</param>
//    private bool ValidateNugetPackage(QueryPackageResponse response)
//    {
//        var foundPackage = response.Data.FirstOrDefault(x => x.Id.Equals(_packageName));
//        if (foundPackage == null)
//        {
//            throw new NugetPackageNotFoundException($"Unable to find nuget package name {_packageName}");
//        }

//        if (foundPackage.Version.Equals(_version))
//        {
//            return true;
//        }

//        return foundPackage.Versions.Any(x => x.Version.Equals(_version));
//    }

//    private Solution ReadSolutionStructure()
//    {
//        var slnFileReader = new SolutionFileReader(_solutionFileName, true);

//        try
//        {
//            var solution = slnFileReader.Read();
//            return solution;
//        }
//        catch (SolutionFileException)
//        {
//            throw;
//        }
//        catch (Exception)
//        {
//            throw;
//        }

//    }
//}
