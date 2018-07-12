## Bootstrapping NuGet

There is a [bootstrapping starter package](https://www.nuget.org/packages/NServiceBus.Bootstrap.WindowsService) on NuGet that automates most of the above code.

NOTE: The `NServiceBus.Bootstrap.WindowsService` package will not work properly using the `PackageReference` in project files, for more information see [nuget documentation](https://docs.microsoft.com/en-us/nuget/consume-packages/package-references-in-project-files). To get it working, use the `Packages.config` NuGet package manager format in Visual Studio 2017 or higher.


### How to use

Create a new console application (**.NET 4.5.2 or higher**) and install the NuGet package. A minimal NServiceBus configuration will be set up along with a `ProgramService.cs` class that can be used as both an interactive console for development purposes and a Windows service for production use.

NOTE: This will also delete the default `Program.cs` since it is superseded by `ProgramService.cs`.


### Single use NuGet

This is a "single use NuGet". So after installing and adding code to the project, it will remove itself. Since it is single use there will never be any "upgrade"; this is a "use and then own the code" approach.


### For new self-hosting applications

This NuGet helps get started on a new self-hosted NServiceBus application. For existing NServiceBus projects, the problems this NuGet attempts to address are likely already solved.

### Transport

The LearningTransport is selected by default.

WARNING: Choose a production-grade transport before deploying to production.


### Persistence

No persistence is needed since the LearningTransport is used and no sagas are present. Consult the individual transport and saga documentation for specific storage need.