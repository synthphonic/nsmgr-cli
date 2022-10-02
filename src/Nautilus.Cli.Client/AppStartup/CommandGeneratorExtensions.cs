internal static class CommandGeneratorExtensions
{
    internal static void BuildNugetPackageCommand(this RootCommand rootCommand)
    {
        var nugetPackageNameOption = OptionGenerator.CreateOption<string>(name: "--name", description: "The nuget package name to find. e.g: Newtonsoft.Json", alias: "-n", isRequired: true);
        var nugetPackageVersionOption = OptionGenerator.CreateOption<string>(name: "--version", description: "The nuget package version to target", alias: "-v", isRequired: true);
        var projectNameOption = OptionGenerator.Common!.ProjectPathOption();
        var solutionPathOption = OptionGenerator.Common.SolutionOption();
        var showErrorOption = OptionGenerator.CreateOption<bool>(name: "--show-error", description: "Display full error", defaultValue: false, alias: "-e", isHidden: true);

        //
        // Parent: package command
        //
        var packageCommand = new Command("package", "Nuget package command");
        rootCommand.Add(packageCommand);

        //
        // package find --name <name>
        //
        var findPackageCommand = new Command("find", "Finds the project(s) that depends on the intended nuget package");
        findPackageCommand.AddOption(solutionPathOption);
        findPackageCommand.AddOption(nugetPackageNameOption);
        findPackageCommand.SetHandler(async (solutionFile, nugetPackageName, showFullError) =>
        {
            //Console.WriteLine($"trying to find {nugetPackageName}");

            try
            {
                var command = new FindPackageCommand(solutionFile, nugetPackageName);
                await command.ExecuteAsync();
            }
            catch (CommandException cmdException)
            {
                ConsoleOutputLayout.DisplayCommandExceptionMessageFormat(cmdException);
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                ConsoleOutputLayout.DisplayExceptionMessageFormat(ex, showFullError);
                Environment.Exit(-1);
            }

        }, solutionPathOption, nugetPackageNameOption, showErrorOption);

        packageCommand.Add(findPackageCommand);

        //
        // package list --solution
        //
        var listPackageSubCommand = new Command("list", "List out all projects that exists under a solution (.sln)");
        listPackageSubCommand.AddOption(solutionPathOption);
        listPackageSubCommand.AddOption(showErrorOption);
        listPackageSubCommand.SetHandler(async (solutionFileInfo, showFullError) =>
        {
            try
            {
                var findPackageCmd = new ListNugetPackagesCommand(solutionFileInfo, false, showFullError);
                await findPackageCmd.ExecuteAsync();
            }
            catch (CommandException cmdException)
            {
                ConsoleOutputLayout.DisplayCommandExceptionMessageFormat(cmdException);
                Environment.Exit(-1);
            }
            catch (Exception ex)
            {
                ConsoleOutputLayout.DisplayExceptionMessageFormat(ex, showFullError);
                Environment.Exit(-1);
            }
        }, solutionPathOption, showErrorOption);

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

        updatePackageSubCommand.AddOption(nugetPackageNameOption);
        updatePackageSubCommand.AddOption(nugetPackageVersionOption);
        updatePackageSubCommand.AddOption(projectNameOption);
        updatePackageSubCommand.AddOption(solutionPathOption);
        updatePackageSubCommand.SetHandler((packageName, packageVersion, projectName, solutionFileInfo, showFullError) =>
        {
            Console.WriteLine($"Package Name    : {packageName}");
            Console.WriteLine($"Version         : {packageVersion}");
            Console.WriteLine($"Project Name    : {projectName}");
            Console.WriteLine($"Solution Path   : {solutionFileInfo.Name} : [{solutionFileInfo.Exists}]");

        }, nugetPackageNameOption, nugetPackageVersionOption, projectNameOption, solutionPathOption, showErrorOption);
    }

    internal static void BuildProjectCommand(this RootCommand rootCommand)
    {
        var solutionOption = OptionGenerator.Common!.SolutionOption();
        var projectOption = OptionGenerator.Common!.ProjectPathOption();
        var backupOption = OptionGenerator.CreateOption<bool>("--backup", "Backup file before version is modified", "-b");
        var restoreOption = OptionGenerator.CreateOption<bool>("--restore", "Restore file to its original state before modification", "-r");
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
        projectListCommand.AddOption(solutionOption);
        projectCommand.Add(projectListCommand);

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
        versionCommand.AddOption(projectOption);
        versionCommand.AddOption(solutionOption);
        versionCommand.AddOption(backupOption);
        versionCommand.AddOption(restoreOption);
        versionCommand.SetHandler((solutionFileInfo, isBackup, isRestore) =>
        {
            Console.WriteLine($"Solution: {solutionFileInfo.Name} : [{solutionFileInfo.Exists}] : backup {isBackup} , restore {isRestore}");

        }, solutionOption, backupOption, restoreOption);

        updateCommand.Add(versionCommand);
        projectCommand.Add(updateCommand);

        BuildProjectMetadataCommand(projectCommand);
    }

    private static void BuildProjectMetadataCommand(Command? parentCommand)
    {
        var metadataOption = OptionGenerator.CreateOption<string>(name: "--xml-metadata", description: "The fully qualified xml metadata element to target", alias: "-x", isRequired: true);
        var projectPathOption = OptionGenerator.Common!.ProjectPathOption();

        //
        // project metadata add
        //  --xml-metadata PropertyGroup:Authors=a,bb,ccc
        //  --project-path ~/projects/somefoldername/myproject.csproj
        //
        var metadataCommand = new Command("metadata", "Visual studio's project file metadata command");
        parentCommand!.Add(metadataCommand);

        var addMetadataCommand = new Command("add", "Add a new project metadata to the project file");
        addMetadataCommand.SetHandler((xmlMetadata, projectPath) =>
        {
            Console.WriteLine($"Execute : add metadata {xmlMetadata} : project-path {projectPath} : [{projectPath.Exists}]");

        }, metadataOption, projectPathOption);

        //
        // project metadata update
        //  --xml-metadata PropertyGroup:Authors=a,bb,ccc
        //  --project-path ~/projects/somefoldername/myproject.csproj
        //
        var updateMetadataCommand = new Command("update", "Update project metadata to the project file");
        updateMetadataCommand.SetHandler((xmlMetadata, projectPath) =>
        {
            Console.WriteLine($"Execute : update metadata {xmlMetadata} : project-path {projectPath} : [{projectPath.Exists}]");

        }, metadataOption, projectPathOption);

        //
        // project metadata delete
        //  --xml-metadata PropertyGroup:Authors
        //  --project-path ~/projects/somefoldername/myproject.csproj
        //
        var deleteMetadataCommand = new Command("delete", "Delete project metadata");
        deleteMetadataCommand.SetHandler((xmlMetadata, projectPath) =>
        {
            Console.WriteLine($"Execute : delete metadata {xmlMetadata} : project-path {projectPath} : [{projectPath.Exists}]");

        }, metadataOption, projectPathOption);

        addMetadataCommand.AddOption(metadataOption);
        addMetadataCommand.AddOption(projectPathOption);

        updateMetadataCommand.AddOption(metadataOption);
        updateMetadataCommand.AddOption(projectPathOption);

        deleteMetadataCommand.AddOption(metadataOption);
        deleteMetadataCommand.AddOption(projectPathOption);

        metadataCommand.Add(addMetadataCommand);
        metadataCommand.Add(updateMetadataCommand);
        metadataCommand.Add(deleteMetadataCommand);
    }
}