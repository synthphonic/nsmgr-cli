# nsmgr cli v2.1


## Available Commands
[find-package](https://github.com/synthphonic/nautilus-cli/blob/33-documentation-update-readmemd-to-include-modify-project-version-command/COMMANDS.md#find-package)
Finds the project(s) that depends on the intended nuget package

[list-projects](https://github.com/synthphonic/nautilus-cli/blob/33-documentation-update-readmemd-to-include-modify-project-version-command/COMMANDS.md#list-projects)
List out all projects that exists under a solution (.sln) file

[list-packages](https://github.com/synthphonic/nautilus-cli/blob/33-documentation-update-readmemd-to-include-modify-project-version-command/COMMANDS.md#list-packages)
List nuget packages for all projects in the solution

[update-nuget-package](https://github.com/synthphonic/nautilus-cli/blob/33-documentation-update-readmemd-to-include-modify-project-version-command/COMMANDS.md#update-nuget-package)
Finds the conflicting nuget package versions installed in the solution.

[modify-project-version](https://github.com/synthphonic/nautilus-cli/blob/33-documentation-update-readmemd-to-include-modify-project-version-command/COMMANDS.md#modify-project-version)
Modify a particular project 'Version' element accordingly.

[project-metadata](https://github.com/synthphonic/nautilus-cli/edit/FEATURES/44-ability-to-addupdatedelete-csproj-file-metadata-related-to-nuget-package-creation/docs/COMMANDS.md#project-metadata)
Add/Delete/Update xml element or csproj metadata in project file.

## find-package
Usage:
```csharp
./nsmgr find-package --package Xamarin.Forms --solutionfile /users/itsme/xxx.sln
```
  - solutionfile :   *Required. The full file path to the .sln file*
  - package          :   *Required. The nuget package name to find.*

## list-projects
Usage:
```csharp
./nsmgr list-projects --solutionfile /users/itsme/xxx.sln
```
  - solutionfile : *Required. The full file path to the .sln file.*
  - projects-only   : *Process project files only and ignore the rest.*
  - nuget-packages : *(Default: false) Display nuget packages for each project.*
  - nuget-package-updates : *(Default: false) Query and display if there's any new nuget package update version online.*

## list-packages
Usage:
```csharp
./nsmgr list-packages --solutionfile /users/itsme/xxx.sln
```
  - solutionfile : *Required. The full file path to the .sln file*


## update-nuget-package
Usage:
```csharp
./nsmgr update-nuget-package --package Xamarin.Forms --project MyProject.Name --solutionfile /users/itsme/xxx.sln --version 3.6.1.21221121
```
  - solutionfile : *Required. The full file path to the .sln file.*
  - project : *Required. The name of the project to update the nuget packages involved. Use 'all' to upgrade all projects*
  - package : *Required. The nuget package name to update.*

## modify-project-version
Usage:
```csharp
./nsmgr modify-project-version -p <project path> -v <newversion> Xamarin.Forms --project MyProject.Name --solutionfile /users/itsme/xxx.sln --version 3.6.1.21221121
```
- **-p, --project-path**. Required. The full file path of a given csproj file name.
- **-v, --version-number**. Required. The new version number.
- **-b, --backup**. Prior to version change, the command should backup the original version number.
- **-r, --restore-version**.  Restore the version number to its original state.

## project-metadata
Usage:
**Example 1: Add/Update the 'Authors' project metadata**
```csharp
nsmgr project-metadata -m PropertyGroup:Authors=aa,bb,cc -p /projects/myproject/xxx.csproj
```
**Example 2: Add/Update the 'Version' project metadata**
```csharp
nsmgr project-metadata -m PropertyGroup:Version=1.0.0.200 -p /projects/myproject/xxx.csproj
```

**Example: Delete project metadata**
```csharp
nsmgr project-metadata -r -m PropertyGroup:Authors -p /projects/myproject/xxx.csproj
```

**Command Parameters**
- **-p, --project-path**. Required. The full file path of a given csproj file name.
- **-m, --xml-metadata**. Required. The fully qualified metadata or xml element name to operate on.

#
**NOTE:** All commands has ***--debug*** switch, with default value to false. If this option is turned on, output errors will be displayed to the console
