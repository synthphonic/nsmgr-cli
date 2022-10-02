﻿namespace Nautilus.Cli.Client.Commands;

internal sealed class ModifyProjectVersionCommand
{
    private readonly FileInfo _projectFile;
    private readonly string _versionNumber;
    private readonly bool _restoreVersionNumber;
    private readonly bool _backup;

    public ModifyProjectVersionCommand(FileInfo projectFile, string versionNumber, bool restoreVersionNumber, bool backup)
    {
        _projectFile = projectFile;
        _versionNumber = versionNumber;
        _restoreVersionNumber = restoreVersionNumber;
        _backup = backup;
    }

    public void Execute()
    {
        Run().Wait();
    }

    private async Task Run()
    {
        Colorful.Console.WriteLine();
        Colorful.Console.WriteLine("Working. Please wait...", Color.GreenYellow);

        var prjMetadata = ProjectMetadata.SetMetadata(_projectFile.Name);
        var project = new Project(prjMetadata);
        project.Read();

        if (_restoreVersionNumber && _backup)
            throw new CommandException("Restore and Backup switch cannot be used together in a single command line usage.");

        if (_restoreVersionNumber)
        {
            project.Restore();
        }
        else if (_backup)
        {
            project.Backup();
            project.Update("Version", _versionNumber);
        }

        await Task.CompletedTask;
    }
}