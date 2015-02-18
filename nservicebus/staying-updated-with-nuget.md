---
title: Staying Updated with NuGet
summary: 'NuGet is an open source project that simplifies integration of third parties into Visual Studio projects during development. '
tags: []
---

NuGet is an open source project that simplifies integration of third parties into your Visual Studio projects during development. To learn more or to download it, go to the [NuGet web site](https://www.nuget.org/).

## Getting the latest stable NServiceBus

After installing NuGet you have a few ways to get NServiceBus using NuGet. Right click your project references and choose "Manage NuGet Packages". Search for "NServiceBus", and select "Install". Alternatively you can get to the NuGet package manager via "Library Package Manage" and selecting "Manage NuGet Packages for Solution".

To install NServiceBus core libraries, open the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console) and type this:

    PM> Install-Package NServiceBus

To use the [NServiceBus hosting process](the-nservicebus-host.md), install the following package, which also installs the NServiceBus core libraries:

    PM> Install-Package NServiceBus.Host

If you already downloaded the latest release and just want to an update, use this:

    PM> Update-Package NServiceBus

NOTE: When resolving dependency version on a package, Nuget by default installs the latest patch version. In order to always use the latest minor version, please use the `-DependencyVersion HighestMinor`. This is available from Nuget 2.8.x onwards. For example, when installing NServiceBus.Host, to always get the latest version of the NServiceBus core, use:
`Install-Package NServiceBus.Host [ProjectName] -DependencyVersion HighestMinor`

## Downloading NServiceBus latest build

The NServiceBus latest build can be downloaded from Particular MyGet feed `https://www.myget.org/F/particular/`. You can add this package source to your list of available [Package Sources](http://docs.nuget.org/consume/package-manager-dialog#package-sources). If pre-releas packages are required the see [Installing Prerelease Packages](http://docs.nuget.org/create/versioning#installing-prerelease-packages).

## Additional NServiceBus NuGet packages

There are additional NServiceBus NuGet packages, to make your integration with NServiceBus even easier. Each package specifies libraries and configures NServiceBus to utilize them.

 * [List of all nugets produced by Particular](http://www.nuget.org/profiles/nservicebus)
 * [List of all nugets that integrate with NServiceBus](http://www.nuget.org/packages?q=nservicebus)

