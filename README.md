# What is Nautilus CLI 
My own personal code playground to implement simple operations on csproj files with relation to nuget packages
Nautilus CLI is (well from the name) is a command line tool.
Just imagine if you have more than 10 projects (*in my case i have several solutions that have 20-40 projects in it*) in a Visual Studio solution, and you want to only update specific nuget packages. It will be a nightmare! 

Perhaps this tool can help you!

Oh btw, There's always room for improvements, after all we are just human beings. We do our best. If there bugs, we fix. 

## Nautilus CLI v1.1.0.0 
v1.1 works for For **Windows x64** and **MacOS X x64**. I haven't tested it on **Linux** as I have no use for it just yet. So if anyone who would like to go ahead and test it, be my guest... :)

## Available commands
**find-package**
Finds the project(s) that depends on the intended nuget package

**list-projects**
List out all projects that exists under a solution (.sln) file

**list-packages**
List nuget packages for all projects in the solution

**update-nuget-package**
Finds the conflicting nuget package versions installed in the solution.

## Command Details

### find-package
Usage:
```csharp
./nsmgr find-package --package Xamarin.Forms --solutionfile /users/itsme/xxx.sln
```
  - solutionfile :   *Required. The full file path to the .sln file*
  - package          :   *Required. The nuget package name to find.*

### list-projects
Usage:
```csharp
./nsmgr list-projects --solutionfile /users/itsme/xxx.sln
```
  - solutionfile : *Required. The full file path to the .sln file.*
  - projects-only   : *Process project files only and ignore the rest.*
  - nuget-packages : *(Default: false) Display nuget packages for each project.*
  - nuget-package-updates : *(Default: false) Query and display if there's any new nuget package update version online.*

### list-packages
Usage:
```csharp
./nsmgr list-packages --solutionfile /users/itsme/xxx.sln
```
  - solutionfile : *Required. The full file path to the .sln file*


### update-nuget-package
Usage:
```csharp
./nsmgr update-nuget-package --package Xamarin.Forms --project MyProject.Name --solutionfile /users/itsme/xxx.sln --version 3.6.1.21221121
```
  - solutionfile : *Required. The full file path to the .sln file.*
  - project : *Required. The name of the project to update the nuget packages involved. Use 'all' to upgrade all projects*
  - package : *Required. The nuget package name to update.*

## Release Status
### v1.1 Current release and status
Current version is **v1.1** which is already obsolete, but still usable. It is not tested with latest .NET Core.

A lot has changed in **v2.1** since it was first published. For example cli executable file name changed from **nautilus-cli** to **nsmgr** and many other items. The changes are needed to support even wider stuff in the future, at the same time preserve existing code base where necessary.

### v2.1 Release status
Refer here [Nautilus CLI v1.1.0.0 win-x64 osx-x64 ](https://github.com/synthphonic/nautilus-cli/releases/tag/Nautilus-CLI-1.1.0.0-winx64-osx64)

|Version| Status |
|--|--|
|nsmgr-v2.1-windows-X64 | Not released yet  |
|nsmgr-v2.1-macosx-X64 | Not released yet  |
|nsmgr-v2.1-macosx-M1 | Future |


#
**NOTE:** All commands has ***--debug*** switch, with default value to false. If this option is turned on, output errors will be written to a log file and stored on the user's _Desktop_ location
