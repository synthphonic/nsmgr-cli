// See https://aka.ms/new-console-template for more information

Console.WriteLine("Nautilus solution manager cli");
Console.WriteLine();

var rootCommand = new RootCommand("Nautilus CLI for Visual Studio based solution manager");
rootCommand.BuildNugetPackageCommand();
rootCommand.BuildProjectCommand();
await rootCommand.InvokeAsync(args);