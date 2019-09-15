# Nautilus CLI v1.1.0.0 
For Windows x64 and MacOS X x64 

### Available verbs
* find-package
Finds the project(s) that depends on the intended nuget package
* list-projects
List out all projects that exists under a solution (.sln) file
* list-packages
List nuget packages for all projects in the solution
* update-nuget-package
Finds the conflicting nuget package versions installed in the solution.


## find-package
Finds the project(s) that depends on the intended nuget package

Usage:
	**nautilus-cli find-package --package Xamarin.Forms --solutionfilename /users/itsme/xxx.sln**

  --solutionfilename    Required. The full file path to the .sln file
  --package             Required. The nuget package name to find.

## list-projects
List out all projects that exists under a solution (.sln) file

Usage:
	**nautilus-cli list-projects --solutionfilename /users/itsme/xxx.sln**
  
  --solutionfilename         Required. The full file path to the .sln file.

  --projects-only            Process project files only and ignore the rest.

  --nuget-packages           (Default: false) Display nuget packages for each project.

  --nuget-package-updates    (Default: false) Query and display if there's any new nuget package update version online.

## list-packages
List nuget packages for all projects in the solution

Usage:
	**nautilus-cli list-packages --solutionfilename /users/itsme/xxx.sln**

  --solutionfilename    Required. The full file path to the .sln file


## update-nuget-package
Finds the conflicting nuget package versions installed in the solution.

Usage:
	**nautilus-cli update-nuget-package --package Xamarin.Forms --project MyProject.Name --solutionfilename /users/itsme/xxx.sln --version 3.6.1.21221121**

  --solutionfilename    Required. The full file path to the .sln file.

  --project             Required. The name of the project to update the nuget packages involved. Use 'all' to upgrade all projects

  --package             Required. The nuget package name to update.
