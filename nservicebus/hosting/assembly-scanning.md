---
title: Assembly scanning
summary: To enable automatic detection of various features NServiceBus scans your assemblies for well known types
tags: []
redirects:
 - nservicebus/assembly-scanning
---

By default, NServiceBus scans all assemblies in the endpoint bin folder to find types implementing its interfaces so that it can configure them automatically. 

Scanning is invoked by default for self-hosting

<!-- import ScanningDefault -->

or when NServiceBus.Host is used

<!-- import ScanningConfigurationInNSBHost -->

NOTE: During the scanning process, the core dlls for NServiceBus namely `NServiceBus.Core.dll`, `NServiceBus.dll` (in versions prior to Version 5) and if in use `NServiceBus.Host.exe` are automatically included since the endpoint needs them to function properly.


## Controlling the assemblies to scan

There are some cases where you need fine grained control over which assemblies are loaded:

- To limit the number of assemblies being scanned and hence provide improvements to startup time.
- If you are hosting multiple endpoints out of the same directory (made possible in Version 5) i.e. each endpoint would want to load a subset of assemblies.
- In versions prior to Version 4.1, non .NET assemblies, e.g. COM dlls might need to be excluded. Starting with Version 4.1, non .NET assemblies are automatically excluded.
 
NOTE: Extensions to NServiceBus (for example `NServiceBus.Distributor.MSMQ.dll` or `NServiceBus.RavenDB.dll`) are not considered core dlls and will need to be explicitly added if you customize assembly scanning.

## Default behaviour

BETA: In version 6 default behaviour for assembly scanning has changed not to scan nested folders for assemblies.

For version 5 and below assemblies in nested folders were automatically scanned by default. 

From version 6, default behaviour is not to scan nested folders for assemblies. You can enable nested folders assembly scanning using:

<!-- ScanningNestedAssebliesEnabled -->

## Excluding assemblies


### You can exclude specific assemblies by name:

<!-- import ScanningExcludeByName -->


### You can exclude specific types:

<!-- import ScanningExcludeTypes -->


## Including assemblies

 
### You can pass assemblies:

<!-- import ScanningListOfAssemblies -->


### And if you need to control the exact types that NServiceBus uses you can pass them in:

<!-- import ScanningListOfTypes -->


## Options deprecated from version 6 and later

Use the `AllAssemblies` helper class to easily create a list of assemblies either by creating a blacklist using the method Except or a whitelist by using Matching or a combination of both.

NOTE: The `Except`, `Matching` and `And` methods behave like `string.StartsWith(string)`.


### Include assemblies using pattern matching:

<!-- import ScanningIncludeByPattern -->


### Mixing includes and excludes:

<!-- import ScanningMixingIncludeAndExclude -->


### Specifying the directory to scan:

<!-- import ScanningCustomDirectory -->

NOTE: Assembly scanning options applied in `INeedInitialization` code will not be applied. Assembly scanning options need to be specified using `IConfigureThisEndpoint` when using NServiceBus.Host or when self hosting and creating `BusConfiguration` only. 
