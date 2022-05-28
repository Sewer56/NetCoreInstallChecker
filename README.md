# NetCoreInstallChecker
![NuGet Badge](https://img.shields.io/nuget/v/NetCoreInstallChecker)

NetCoreInstallChecker is a miniature personal use `netstandard2.0` library that allows for the parsing of .NET Core runtime configurations and checking whether the necessary versions of Core are installed to run an application.

The goal is to run either a .NET Framework application on Windows, or even better, an application built using [CoreRT](https://github.com/dotnet/corert/tree/master/samples/HelloWorld) to quickly check if .NET Core is installed prior to running the actual Core application. 

Note: This library specifically targets Windows. It should theoretically also run on Linux and OSX, however this has not been explicitly tested. 

### Supported Features

- Detect all globally installed frameworks.
- Generate URL to framework download link.
- Roll Forward Policies (Core 3+).
- Multiple required frameworks in `RuntimeConfig.json` (.NET 6.0+)
- Minimal Parsing of `runtimeconfig.json`.
- Targets the official spec at [dotnet/runtime/framework-version-resolution.md](https://github.com/dotnet/runtime/blob/main/docs/design/features/framework-version-resolution.md).

### Goals

- Improve end-user experience.
- Reduce update size for small applications (shared framework).
- Ship applications where self-contained deployment may not be optimal (e.g. [Using custom Core runtime hosting](https://docs.microsoft.com/en-us/dotnet/core/tutorials/netcore-hosting))

This library was made with self-updating applications such as [Reloaded-II](https://github.com/Reloaded-Project/Reloaded-II/) in mind, due to the frustration of having end users ignoring dependencies listed on the download page (even when clearly stated right above the download button). 

### Basic Usage

##### Find Installed Frameworks

```csharp
var finder     = new FrameworkFinder(is64Bit);
var frameworks = finder.GetFrameworks();

/*
    frameworks:
        Microsoft.AspNetCore.All
        Microsoft.AspNetCore.App
        Microsoft.NETCore.App
        Microsoft.WindowsDesktop.App
*/
```

##### Resolve Dependencies

```csharp
// runtimeConfigPath: Path to runtimeconfig.json
var finder    = new FrameworkFinder(is64Bit);
var resolver  = new DependencyResolver(finder);
var result    = resolver.Resolve(RuntimeOptions.FromFile(runtimeConfigPath));

// Check if dependencies are missing.
if (!result.Available) 
{
    // Do something with missing dependencies.
    // For example:
    foreach (var dependency in result.MissingDependencies)
    {
        var downloader = new FrameworkDownloader(dependency.NuGetVersion, dependency.FrameworkName);
        Console.WriteLine($"Framework {dependency.Name} required to run this application is missing.");
        Console.WriteLine($"You can download it using the following URL {await downloader.GetDownloadUrlAsync(Architecture.x86)}");
    }
}
```

This is a basic example, for good user experience it is recommended you create a window (e.g. using [Avalonia](https://github.com/AvaloniaUI/Avalonia)) to display this information in a nicely grouped manner.
