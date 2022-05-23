# What is Nautilus CLI 
My own personal code playground to implement simple operations on csproj files with relation to nuget packages
Nautilus CLI is (well from the name) is a command line tool.
Just imagine if you have 10-15 projects in a Visual Studio solution, and you want to only update specific nuget packages. It will be a nightmare! 

Perhaps this tool can help you!

## Nautilus CLI v1.1.0.0 
For Windows x64 and MacOS X x64 

### Available verbs
**find-package**
Finds the project(s) that depends on the intended nuget package

**list-projects**
List out all projects that exists under a solution (.sln) file

**list-packages**
List nuget packages for all projects in the solution

**update-nuget-package**
Finds the conflicting nuget package versions installed in the solution.

# Verb Details

### find-package
Finds the project(s) that depends on the intended nuget package

Usage:
    **./nsmgr find-package --package Xamarin.Forms --solutionfile /users/itsme/xxx.sln**
  - solutionfile :   *Required. The full file path to the .sln file*
  - package          :   *Required. The nuget package name to find.*

### list-projects
List out all projects that exists under a solution (.sln) file

Usage:
    **./nsmgr list-projects --solutionfile /users/itsme/xxx.sln**
  - solutionfile : *Required. The full file path to the .sln file.*
  - projects-only   : *Process project files only and ignore the rest.*
  - nuget-packages : *(Default: false) Display nuget packages for each project.*
  - nuget-package-updates : *(Default: false) Query and display if there's any new nuget package update version online.*

### list-packages
List nuget packages for all projects in the solution

Usage:
    **./nsmgr list-packages --solutionfile /users/itsme/xxx.sln**
  - solutionfile : *Required. The full file path to the .sln file*


### update-nuget-package
Finds the conflicting nuget package versions installed in the solution.

Usage:
    **./nsmgr update-nuget-package --package Xamarin.Forms --project MyProject.Name --solutionfile /users/itsme/xxx.sln --version 3.6.1.21221121**

  - solutionfile : *Required. The full file path to the .sln file.*
  - project : *Required. The name of the project to update the nuget packages involved. Use 'all' to upgrade all projects*
  - package : *Required. The nuget package name to update.*
#
#
**NOTE:** All verbs has _--debug_ option. Default value is false. If this option is turned on, output errors will be written to a log file and stored on the user's _Desktop_ location
