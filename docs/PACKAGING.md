# Packaging & Publishing the CLI tool

## Publish an app self-contained and ReadyToRun.

### macOS x64 
- Publish an app self-contained and ReadyToRun. A macOS 64-bit executable is created.
    ```csharp
    dotnet publish -c Release -r osx-x64 -p:PublishReadyToRun=true
    ```
- Publish a self-contained and ReadyToRun and single file.
    ```csharp
    dotnet publish -c Release -r osx-x64 -o publish-x64 -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true -p:UseAppHost=true
    ```
### macOS x64 Monterey
- Publish an app self-contained and ReadyToRun. A macOS 64-bit executable is created.
    ```csharp
    dotnet publish -c Release -r osx.12-x64 -p:PublishReadyToRun=true
    ```
- Publish a self-contained and ReadyToRun and single file.
    ```csharp
    dotnet publish -c Release -r osx.12-x64 -o publish-12-x64 -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true -p:UseAppHost=true
    ```

### macOS M1
- Publish an app self-contained and ReadyToRun. A macOS 64-bit executable is created.
    ```csharp
    dotnet publish -c Release -r osx.12-arm64 -p:PublishReadyToRun=true
    ```
- Publish a self-contained and ReadyToRun and single file.
    ```csharp
    dotnet publish -c Release -r osx.12-arm64 -o publish-arm64 -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true -p:UseAppHost=true
    ```

### Windows 64-bit
- Publish an app self-contained and ReadyToRun. A Windows 64-bit executable is created
    ```csharp
    dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=true
    ```

- Publish a self-contained and ReadyToRun and single file.
    ```csharp
    dotnet publish -c Release -r win-x64 -o publish -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true -p:IncludeNativeLibrariesForSelfExtract=true
    ```



## Reference
- [Application publishing - .NET | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/core/deploying/)
- [Single file application - .NET | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file/overview#other-considerations)
- [designs/design.md at main · dotnet/designs (github.com)](https://github.com/dotnet/designs/blob/main/accepted/2020/single-file/design.md#user-experience)
- [c# - .Net 5 Publish Single File - Produces exe and dlls - Stack Overflow](https://stackoverflow.com/questions/65170327/net-5-publish-single-file-produces-exe-and-dlls)
