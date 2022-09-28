// See https://aka.ms/new-console-template for more information

Console.WriteLine("Nautilus solution manager cli");
Console.WriteLine();

var rootCommand = new RootCommand("Nautilus CLI for Visual Studio based solution manager");
rootCommand.BuildPackageCommand();


//
// project 
//
//var projectCommand = new Command("project", "Project command");
//root.Add(projectCommand);

//var metadata_project_subCommand = new Command("metadata", "");
//projectCommand.Add(metadata_project_subCommand);

//var add_metadata_project_subCommand = new Command("add", "Add xml element or csproj metadata in project file");
//metadata_project_subCommand.Add(add_metadata_project_subCommand);


await rootCommand.InvokeAsync(args);