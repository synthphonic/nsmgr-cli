﻿internal static class CommandGeneratorExtensions
{
    private static Option<string> NugetPackageNameOption = OptionGenerator.CreateOption<string>(name: "--package-name", description: "The nuget package name to find. e.g: Newtonsoft.Json", alias: "-n", isRequired: true);
    private static Option<string> NugetPackageVersionOption = OptionGenerator.CreateOption<string>(name: "--version", description: "The nuget package version to target", alias: "-v", isRequired: true);
    private static Option<FileInfo> SolutionPathOption = OptionGenerator.Common!.SolutionPathOption();
    private static Option<FileInfo> ProjectPathOption = OptionGenerator.Common!.ProjectPathOption();
    private static Option<string> ProjectNameOption = OptionGenerator.Common!.ProjectNameOption();
    private static Option<bool> BackupOption = OptionGenerator.CreateOption<bool>("--backup", "Backup file before version is modified", "-b");
    private static Option<bool> RestoreOption = OptionGenerator.CreateOption<bool>("--restore", "Restore file to its original state before modification", "-r");
    private static Option<bool> ShowFullErrorOption = OptionGenerator.CreateOption<bool>(name: "--show-error", description: "Display full error", defaultValue: false, alias: "-e", isHidden: true);
    private static Option<bool> ShowPreReleasePackageOption = OptionGenerator.CreateOption<bool>(name: "--prerelease-package", description: "Show pre release nuget package", defaultValue: false, alias: "-r", isHidden: false);
    private static Option<bool> ShowProjectOnlyOption = OptionGenerator.CreateOption<bool>(name: "--project-only", description: "Show projects only", defaultValue: false, alias: "-p");
    private static Option<bool> ShowNugetPackageUpdateOption = OptionGenerator.CreateOption<bool>(name: "--package-update", description: "Show available nuget package updates", defaultValue: false, alias: "-u");
    private static Option<string> XmlMetadataOption = OptionGenerator.CreateOption<string>(name: "--xml-metadata", description: "The fully qualified xml metadata element to target", alias: "-x", isRequired: true);

    internal static void BuildNugetPackageCommand(this RootCommand rootCommand)
    {
        //
        // Parent: package command
        //
        var packageCommand = new Command("package", "Nuget package command");
        rootCommand.Add(packageCommand);

        //
        // package find --name <name>
        //
        var findPackageCommand = new Command("find", "Finds the project(s) that depends on the intended nuget package");
        findPackageCommand.AddOption(SolutionPathOption);
        findPackageCommand.AddOption(NugetPackageNameOption);
        findPackageCommand.SetHandler(async (solutionFile, nugetPackageName, showFullError) =>
        {
            try
            {
                var command = new FindPackageCommand(solutionFile, nugetPackageName);
                await command.ExecuteAsync();
            }
            catch (CommandException cmdException)
            {
                ConsoleOutputLayout.DisplayCommandExceptionMessageFormat(cmdException, showFullError);
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                ConsoleOutputLayout.DisplayExceptionMessageFormat(ex, showFullError);
                Environment.Exit(-1);
            }

        }, SolutionPathOption, NugetPackageNameOption, ShowFullErrorOption);

        packageCommand.Add(findPackageCommand);

        //
        // package list --solution
        //
        var listPackageSubCommand = new Command("list", "List out all projects that exists under a solution (.sln)");
        listPackageSubCommand.AddOption(SolutionPathOption);
        listPackageSubCommand.AddOption(ShowFullErrorOption);
        listPackageSubCommand.SetHandler(async (solutionFileInfo, showFullError) =>
        {
            try
            {
                var findPackageCmd = new ListNugetPackagesCommand(solutionFileInfo, false, showFullError);
                await findPackageCmd.ExecuteAsync();
            }
            catch (CommandException cmdException)
            {
                ConsoleOutputLayout.DisplayCommandExceptionMessageFormat(cmdException, showFullError);
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                ConsoleOutputLayout.DisplayExceptionMessageFormat(ex, showFullError);
                Environment.Exit(-1);
            }
        }, SolutionPathOption, ShowFullErrorOption);

        packageCommand.Add(listPackageSubCommand);

        //
        // package update
        //  --name Xamarin.Forms
        //  --version 1.2.3.4
        //  --project MyProject
        //  --solution /path/to/the/solution/file/mysolution.sln
        //
        var updatePackageSubCommand = new Command("update", "Update a nuget package for a project to a latest version");
        packageCommand.Add(updatePackageSubCommand);

        updatePackageSubCommand.AddOption(NugetPackageNameOption);
        updatePackageSubCommand.AddOption(ShowFullErrorOption);
        updatePackageSubCommand.AddOption(NugetPackageVersionOption);
        updatePackageSubCommand.AddOption(ProjectNameOption);
        updatePackageSubCommand.AddOption(SolutionPathOption);
        updatePackageSubCommand.SetHandler(async (packageName, packageVersion, projectName, solutionFile, showFullError) =>
        {
            Console.WriteLine($"Package Name    : {packageName}");
            Console.WriteLine($"Version         : {packageVersion}");
            Console.WriteLine($"Project Name    : {projectName}");
            Console.WriteLine($"Solution Path   : {solutionFile.Name} : [{solutionFile.Exists}]");

            try
            {
                var cmd = new UpdateNugetPackageCommand(solutionFile, projectName, packageName, packageVersion);
                await cmd.ExecuteAsync();
            }
            catch (CommandException cmdException)
            {
                ConsoleOutputLayout.DisplayCommandExceptionMessageFormat(cmdException, showFullError);
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                ConsoleOutputLayout.DisplayExceptionMessageFormat(ex, showFullError);
                Environment.Exit(-1);
            }

        }, NugetPackageNameOption, NugetPackageVersionOption, ProjectNameOption, SolutionPathOption, ShowFullErrorOption);
    }

    internal static void BuildProjectCommand(this RootCommand rootCommand)
    {
        var argument = new Argument<string>();
        argument.Name = "version number";

        //
        // Parent: project command
        //
        var projectCommand = new Command("project", "Project or csproj command");
        rootCommand.Add(projectCommand);

        //
        // project list
        //  --solution /path/to/the/solution/file/mysolution.sln
        //
        var projectListCommand = new Command("list", "Show all projects in a solution");
        projectListCommand.AddOption(SolutionPathOption);
        projectListCommand.AddOption(ShowProjectOnlyOption);
        projectListCommand.AddOption(ShowNugetPackageUpdateOption);
        projectListCommand.AddOption(ShowFullErrorOption);
        projectListCommand.AddOption(ShowPreReleasePackageOption);
        projectCommand.Add(projectListCommand);
        projectListCommand.SetHandler(async (solutionFile, showProjectOnly, showNugetPackageUpdate, showFullError, showPreReleasePackage) =>
        {
            try
            {
                var command = new ListProjectsCommand(solutionFile, showProjectOnly, showNugetPackageUpdate, showPreReleasePackage);
                await command.ExecuteAsync();
            }
            catch (CommandException cmdException)
            {
                ConsoleOutputLayout.DisplayCommandExceptionMessageFormat(cmdException, showFullError);
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                ConsoleOutputLayout.DisplayExceptionMessageFormat(ex, showFullError);
                Environment.Exit(-1);
            }
        }, SolutionPathOption, ShowProjectOnlyOption, ShowNugetPackageUpdateOption, ShowFullErrorOption, ShowPreReleasePackageOption);

        //
        // project update version 1.2.3.4
        //  --project myproject.csproj
        //  --solution /path/to/the/solution/file/mysolution.sln
        //  --backup (optional)
        //  --restore (optional)
        //
        var updateCommand = new Command("update", "Update command");
        var versionCommand = new Command("version", "The version to use");
        versionCommand.AddArgument(argument);
        versionCommand.AddOption(ProjectNameOption);
        versionCommand.AddOption(SolutionPathOption);
        versionCommand.AddOption(BackupOption);
        versionCommand.AddOption(RestoreOption);
        versionCommand.SetHandler((solutionFileInfo, projectName, isBackup, isRestore) =>
        {
            Console.WriteLine($"Solution: {solutionFileInfo.Name} : [{solutionFileInfo.Exists}] : backup {isBackup} , restore {isRestore}");

        }, SolutionPathOption, ProjectNameOption, BackupOption, RestoreOption);

        updateCommand.Add(versionCommand);
        projectCommand.Add(updateCommand);

        BuildProjectMetadataCommand(projectCommand);
    }

    private static void BuildProjectMetadataCommand(Command? parentCommand)
    {
        //
        // project metadata add
        //  --xml-metadata PropertyGroup:Authors=a,bb,ccc
        //  --project-path ~/projects/somefoldername/myproject.csproj
        //
        var metadataCommand = new Command("metadata", "Visual studio's project file metadata command");
        parentCommand!.Add(metadataCommand);

        var addMetadataCommand = new Command("add", "Add a new project metadata to the project file");
        addMetadataCommand.AddOption(XmlMetadataOption);
        addMetadataCommand.AddOption(ProjectPathOption);
        addMetadataCommand.SetHandler((xmlMetadata, projectPath) =>
        {
            Console.WriteLine($"Execute : add metadata {xmlMetadata} : project-path {projectPath} : [{projectPath.Exists}]");

        }, XmlMetadataOption, ProjectPathOption);

        //
        // project metadata update
        //  --xml-metadata PropertyGroup:Authors=a,bb,ccc
        //  --project-path ~/projects/somefoldername/myproject.csproj
        //
        var updateMetadataCommand = new Command("update", "Update project metadata to the project file");
        updateMetadataCommand.SetHandler((xmlMetadata, projectPath) =>
        {
            Console.WriteLine($"Execute : update metadata {xmlMetadata} : project-path {projectPath} : [{projectPath.Exists}]");

        }, XmlMetadataOption, ProjectPathOption);

        //
        // project metadata delete
        //  --xml-metadata PropertyGroup:Authors
        //  --project-path ~/projects/somefoldername/myproject.csproj
        //
        var deleteMetadataCommand = new Command("delete", "Delete project metadata");
        deleteMetadataCommand.SetHandler((xmlMetadata, projectPath) =>
        {
            Console.WriteLine($"Execute : delete metadata {xmlMetadata} : project-path {projectPath} : [{projectPath.Exists}]");

        }, XmlMetadataOption, ProjectPathOption);

        addMetadataCommand.AddOption(XmlMetadataOption);
        addMetadataCommand.AddOption(ProjectPathOption);

        updateMetadataCommand.AddOption(XmlMetadataOption);
        updateMetadataCommand.AddOption(ProjectPathOption);

        deleteMetadataCommand.AddOption(XmlMetadataOption);
        deleteMetadataCommand.AddOption(ProjectPathOption);

        metadataCommand.Add(addMetadataCommand);
        metadataCommand.Add(updateMetadataCommand);
        metadataCommand.Add(deleteMetadataCommand);
    }
}