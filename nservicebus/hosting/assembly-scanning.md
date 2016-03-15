---
title: Assembly scanning
summary: To enable automatic detection of various features NServiceBus scans assemblies for well known types
tags:
- Assembly scanning
redirects:
 - nservicebus/assembly-scanning
---

By default, NServiceBus scans all assemblies in the endpoint bin folder to find types implementing its interfaces so that it can configure them automatically.

Scanning is invoked by default for self-hosting

snippet:ScanningDefault

or when NServiceBus.Host is used

snippet:ScanningConfigurationInNSBHost

NOTE: During the scanning process, the core dlls for NServiceBus namely `NServiceBus.Core.dll`, `NServiceBus.dll` (in versions prior to Version 5) and if in use `NServiceBus.Host.exe` are automatically included since the endpoint needs them to function properly.

When the endpoint is hosted using NServiceBus.Host, [command line switches](nservicebus-host/#controlling-assembly-scanning-using-the-command-line) can be used to control assembly scanning.

## Controlling the assemblies to scan

There are some cases where you need fine grained control over which assemblies are loaded:

- To limit the number of assemblies being scanned and hence provide improvements to startup time.
- If you are hosting multiple endpoints out of the same directory (made possible in Version 5) i.e. each endpoint would want to load a subset of assemblies.
- In versions prior to Version 4.1, non .NET assemblies, e.g. COM dlls might need to be excluded. Starting with Version 4.1, non .NET assemblies are automatically excluded.

NOTE: Extensions to NServiceBus (for example `NServiceBus.Distributor.MSMQ.dll` or `NServiceBus.RavenDB.dll`) are not considered core dlls and will need to be explicitly added if you customize assembly scanning.


## Nested Directories

In Version 6 and above the default behavior is **not** to scan nested folders for assemblies. You can enable nested folders assembly scanning using:

snippet:ScanningNestedAssebliesEnabled

In Version 5 and below assemblies in nested folders are scanned by default.


## Assemblies to scan

In Version 5 and earlier the API for assembly scanning took an "Include a list" approach. This proved to be problematic. Many extensions to NServiceBus rely on assembly scanning, for example transports and persistences in external NuGets. If, at endpoint configuration time, a list of assemblies was generated, and that list did not include extension assemblies, the endpoint would fail at runtime with some unexpected and hard to diagnose behaviors.

In Version 6 the API has been changes to an "Exclude a list" approach. This supports that the common scenario removing specific assemblies from scanning without the common side effect of accidentally excluding required assemblies.


## Exclude a list approach


### You can exclude specific assemblies by name:

snippet:ScanningExcludeByName


### You can exclude specific types:

snippet:ScanningExcludeTypes


## Include a list approach

Note: These options are deprecated from Version 6 and above.


### Including assemblies:

snippet:ScanningListOfAssemblies


### Controlling the exact types that NServiceBus uses:

snippet:ScanningListOfTypes


### Including assemblies using pattern matching:

snippet:ScanningIncludeByPattern

`AllAssemblies` helper class can be used to create a list of assemblies either by creating a blacklist using the method Except or a whitelist by using Matching or a combination of both.

NOTE: The `Except`, `Matching` and `And` methods behave like `string.StartsWith(string)`.


### Mixing includes and excludes:

snippet:ScanningMixingIncludeAndExclude


### Specifying the directory to scan:

snippet:ScanningCustomDirectory