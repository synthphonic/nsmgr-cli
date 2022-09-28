

# nsmgr cli v3


## Available Commands
## `package Command`
`$ nsmgr package find --name Newtonsoft.Json`

`$ nsmgr package list --solution ~/projects/myproject.sln`

`$ nsmgr package update --name Xamarin.Forms --version 1.2.3.4 --project MyProject.ClientApp --solution ~/projects/my-solution.sln`

`$ nsmgr package metadata add --project ~/projects/myproject/xxx.csproj`

`$ nsmgr package metadata remove --project ~/projects/myproject/xxx.csproj`

## `project Command`

`$ nsmgr project list --solution ~/users/itsme/xxx.sln`

`$ nsmgr project version update -project <project-name> --new-version <newversion> --solution /projects/mysolution.sln`--backup --restore

NOTES:
* *--backup - optional argument*
* *--restore - optional argument*

`$ nsmgr project metadata add --xml-metadata PropertyGroup:Authors=aa,bb,cc --project /projects/myproject.csproj` 

`$ nsmgr project metadata update --xml-metadata PropertyGroup:Version=1.0.0.200 --project /projects/myproject.csproj` 

`$ nsmgr project metadata delete --xml-metadata PropertyGroup:Authors --project /projects/myproject.csproj` 

#
**NOTE:** All commands has ***--debug*** switch, with default value to false. If this option is turned on, output errors will be displayed to the console
