internal static class CommandGeneratorExtensions
{
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
        var find_package_subCommand = new Command("find", "Finds the project(s) that depends on the intended nuget package");
        var find_package_nameOption = OptionGenerator.CreateOption<string>("--name", "The nuget package name to find. e.g: Newtonsoft.Json", "-n", true);

        find_package_subCommand.AddOption(find_package_nameOption);
        find_package_subCommand.SetHandler((nugetPackageName) =>
        {
            Console.WriteLine($"trying to find {nugetPackageName}");
        }, find_package_nameOption);

        packageCommand.Add(find_package_subCommand);

        //
        // package list --solution
        //
        var list_package_subCommand = new Command("list", "List out all projects that exists under a solution (.sln) file");
        var solution_name_option = OptionGenerator.CreateOption<FileInfo>("--solution", "The solution (.sln) full path", "-s", true);
        list_package_subCommand.AddOption(solution_name_option);
        list_package_subCommand.SetHandler((solutionFileInfo) =>
        {
            Console.WriteLine($"trying to find solution name: {solutionFileInfo.Name} : [{solutionFileInfo.Exists}]");
        }, solution_name_option);

        packageCommand.Add(list_package_subCommand);

        //
        // package update
        //  --name Xamarin.Forms
        //  --version 1.2.3.4
        //  --project MyProject
        //  --solution /path/to/the/solution/file/mysolution.sln
        //
        var update_package_subCommand = new Command("update", "Update a nuget package for a project to a latest version");
        packageCommand.Add(update_package_subCommand);

        var nugetPackageNameOption = OptionGenerator.CreateOption<string>("--name", "The nuget package name", "-n", true);
        var nugetPackageVersionOption = OptionGenerator.CreateOption<string>("--version", "The nuget package version to target", "-v", true);
        var projectNameOption = OptionGenerator.CreateOption<string>("--project", "The project name", "-p", true);
        var solutionPathOption = OptionGenerator.CreateOption<FileInfo>("--solution", "The solution path", "-s", true);

        update_package_subCommand.AddOption(nugetPackageNameOption);
        update_package_subCommand.AddOption(nugetPackageVersionOption);
        update_package_subCommand.AddOption(projectNameOption);
        update_package_subCommand.AddOption(solutionPathOption);
        update_package_subCommand.SetHandler((packageName, packageVersion, projectName, solutionFileInfo) =>
        {
            Console.WriteLine($"Package Name    : {packageName}");
            Console.WriteLine($"Version         : {packageVersion}");
            Console.WriteLine($"Project Name    : {projectName}");
            Console.WriteLine($"Solution Path   : {solutionFileInfo.Name} : [{solutionFileInfo.Exists}]");

        }, nugetPackageNameOption, nugetPackageVersionOption, projectNameOption, solutionPathOption);
    }

    internal static void BuildProjectCommand(this RootCommand rootCommand)
    {
        //
        // Parent: project command
        //
        var projectCommand = new Command("project", "Project or csproj command");
        rootCommand.Add(projectCommand);

        //
        // project list
        //  --solution /path/to/the/solution/file/mysolution.sln
        //
        var project_list_command = new Command("list", "Show all projects in a solution");
        var solution_option = OptionGenerator.CreateOption<FileInfo>("--solution", "The solution path", "-s", true);
        project_list_command.AddOption(solution_option);
        projectCommand.Add(project_list_command);

        //
        // project update version 1.2.3.4
        //  --project myproject.csproj
        //  --solution /path/to/the/solution/file/mysolution.sln
        //  --backup (optional)
        //  --restore (optional)
        //

        var updateCommand = new Command("update", "Update command");
        var versionCommand = new Command("version", "The version to use");
        var projectOption = OptionGenerator.CreateOption<string>("--project", "The project name to target", "-p", true);
        var solutionOption = OptionGenerator.CreateOption<FileInfo>("--solution", "The solution path", "-s", true);
        var backupOption = OptionGenerator.CreateOption<bool>("--backup", "Backup file before version is modified", "-b");
        var restoreOption = OptionGenerator.CreateOption<bool>("--restore", "Restore file to its original state before modification", "-r");
        var argument = new Argument<string>();
        argument.Name = "version number";

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
    }
}