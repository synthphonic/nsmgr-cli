namespace Nautilus.Cli.Client.Commands;

internal sealed class ListNugetPackageInSingleProjectCommand : CommandBase
{
    private readonly FileInfo _projectInfo;
    private readonly bool _showNugetUpdates;
    private readonly bool _showPreReleaseNugetPackages;

    public ListNugetPackageInSingleProjectCommand(FileInfo projectInfo, bool showNugetUpdates, bool showPreReleaseNugetPackages, bool returnResults = false)
    {
        _projectInfo = projectInfo;
        _showNugetUpdates = showNugetUpdates;
        _showPreReleaseNugetPackages = showPreReleaseNugetPackages;
    }

    public override async Task ExecuteAsync()
    {
        await RunAsync();
    }

    private async Task RunAsync()
    {
        if (!_projectInfo.Exists)
        {
            throw new CommandException("File does not exists in the given location");
        }

        if (!_projectInfo.Extension.Equals(".csproj"))
        {
            throw new CommandException("Invalid project file");
        }

        Project? project = null;

        try
        {
            var metadata = ProjectMetadata.SetMetadata(_projectInfo.FullName);
            project = new Project(metadata);
            project.Read();

            //Colorful.Console.Write($"{CliStringFormatter.Format15}", "Project ");
            //Colorful.Console.WriteLine($": {project.ProjectFile.FullName}", Color.PapayaWhip);
        }
        catch (SolutionFileException solEx)
        {
            throw new CommandException(solEx.Message, solEx);
        }
        catch (Exception)
        {
            throw;
        }

        NugetPackageOnlineComparer comparer = null!;
        if (_showNugetUpdates)
        {
            comparer = new NugetPackageOnlineComparer(project, _showPreReleaseNugetPackages, CliStringFormatter.WriteOnlinePackageProgressHandler);
            await comparer.Run();
        }

        Colorful.Console.WriteLine();

        WriteFinalizedResultToScreen(project, comparer!);
    }

    private void WriteFinalizedResultToScreen(Project project, NugetPackageOnlineComparer? comparer)
    {
        var projectCounter = 1;
        Colorful.Console.Write($"{CliStringFormatter.Format1}. ", projectCounter);

        if (project.ProjectType == SolutionProjectElement.CSharpProject)
        {
            Colorful.Console.Write(CliStringFormatter.Format54, Color.YellowGreen, $"{project.ProjectName} ({project.Packages.Count()})");
            Colorful.Console.Write($"[{CliStringFormatter.Format5}]", Color.Chocolate, project.TargetFramework);
            Colorful.Console.WriteLine();

            if (project.TargetFramework == ProjectTargetFramework.Unknown)
            {
                // Todo: do something here? 
            }

            if (_showNugetUpdates)
            {
                DisplayNugetPackages(project, comparer!.Result);
            }
            else
            {
                DisplayNugetPackages(project);
            }

            Colorful.Console.WriteLine();
        }
        else
        {
            Colorful.Console.Write(CliStringFormatter.Format45, Color.YellowGreen, $"{project.ProjectName}");

            if (project.TargetFramework == ProjectTargetFramework.Unknown)
            {
                Colorful.Console.Write($"[{CliStringFormatter.Format7}-", Color.Chocolate, project.TargetFramework);
                Colorful.Console.WriteLine($"{CliStringFormatter.Format9}]", Color.Chocolate, project.ProjectType);
            }
            else
            {
                Colorful.Console.WriteLine($"[{CliStringFormatter.Format5}]", project.TargetFramework);
            }
        }

        projectCounter++;

    }

    private void WriteFinalizedResultToScreen2(Project project, Dictionary<string, IList<NugetPackageInformationComparer>> packageVersionComparer)
    {
        var projectCounter = 1;
        Colorful.Console.Write($"{CliStringFormatter.Format1}. ", projectCounter);

        if (project.ProjectType == SolutionProjectElement.CSharpProject)
        {
            Colorful.Console.Write(CliStringFormatter.Format54, Color.YellowGreen, $"{project.ProjectName} ({project.Packages.Count()})");
            Colorful.Console.Write($"[{CliStringFormatter.Format5}]", Color.Chocolate, project.TargetFramework);
            Colorful.Console.WriteLine();

            if (project.TargetFramework == ProjectTargetFramework.Unknown)
            {
                // Todo: do something here? 
            }

            if (_showNugetUpdates)
            {
                DisplayNugetPackages(project, packageVersionComparer);
                Colorful.Console.WriteLine();
            }
        }
        else
        {
            Colorful.Console.Write(CliStringFormatter.Format45, Color.YellowGreen, $"{project.ProjectName}");

            if (project.TargetFramework == ProjectTargetFramework.Unknown)
            {
                Colorful.Console.Write($"[{CliStringFormatter.Format7}-", Color.Chocolate, project.TargetFramework);
                Colorful.Console.WriteLine($"{CliStringFormatter.Format9}]", Color.Chocolate, project.ProjectType);
            }
            else
            {
                Colorful.Console.WriteLine($"[{CliStringFormatter.Format5}]", project.TargetFramework);
            }
        }

        projectCounter++;

    }

    private static void DisplayNugetPackages(Project project, Dictionary<string, IList<NugetPackageInformationComparer>> packageVersionComparer = null!)
    {
        foreach (var package in project.Packages)
        {
            Colorful.Console.Write($"\t{CliStringFormatter.Format50}", Color.Chocolate, package.PackageName);
            Colorful.Console.Write($"{CliStringFormatter.Format30}", Color.YellowGreen, package.Version);

            if (packageVersionComparer != null)
            {
                var nugetPackageInformation = packageVersionComparer[project.ProjectName].FirstOrDefault(x => x.PackageName.Equals(package.PackageName));

                // if nugetPackageInformation is null, then try ToLower() the PackageName, then find it again
                if (nugetPackageInformation == null)
                {
                    nugetPackageInformation = packageVersionComparer[project.ProjectName].FirstOrDefault(x => x.PackageName.ToLower().Equals(package.PackageName));
                }

                if (nugetPackageInformation == null) continue;

                var latestVersionMessage = string.Empty;
                Color color = Color.Green;

                if (nugetPackageInformation.OnlinePackageExists)
                {
                    if (nugetPackageInformation.IsLatestVersion)
                    {
                        latestVersionMessage = "[OK]";
                        color = Color.SeaGreen;
                    }
                    else
                    {
                        latestVersionMessage = $"{nugetPackageInformation.OnlineVersion}";
                        color = Color.OrangeRed;
                    }
                }
                else
                {
                    latestVersionMessage = $"[Skipped]";
                    color = Color.Yellow;
                }

                Colorful.Console.WriteLine($"{CliStringFormatter.Format5}", color, latestVersionMessage);
            }
            else
            {
                Colorful.Console.WriteLine();
            }
        }
    }
}