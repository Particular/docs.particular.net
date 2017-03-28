---
title: Assembly scanning
summary: To enable automatic detection of various features NServiceBus scans assemblies for well known types
reviewed: 2017-03-17
component: core
redirects:
 - nservicebus/assembly-scanning
---

By default all assemblies in the endpoint bin directory are scanned to find types implementing its interfaces so that it can configure them automatically.

NOTE: During the scanning process, the core assembly for NServiceBus (`NServiceBus.Core.dll`) is automatically included since it is required for endpoints to properly function.


## Controlling the assemblies to scan

There are some cases where finer control over which assemblies are loaded is required:

 * To limit the number of assemblies being scanned and hence provide improvements to startup time.
 * If hosting multiple endpoints out of the same directory each endpoint may require a subset of assemblies to be loaded.

NOTE: Extensions (for example `NServiceBus.RavenDB.dll`) are not considered a core assembly and will need to be included when customizing assembly scanning.


## Nested Directories

partial: nested


## Assemblies to scan

partial: assemblies-to-scan


## Exclude a list approach


### Exclude specific assemblies by name:

snippet: ScanningExcludeByName


partial: wildcard


### Exclude specific types:

snippet: ScanningExcludeTypes


partial: include

partial: mixing


## AppDomain assemblies

NOTE: This configuration option is only available in NServiceBus 6.2 and above.

By default, NServiceBus does not scan assemblies already loaded into the AppDomain without being present in the applications base directory. The endpoint can be configured to also scan AppDomain assemblies using:

snippet:ScanningApDomainAssemblies


## Suppress scanning exceptions

NOTE: This configuration option is only available in NServiceBus 6.2 and above.

By default, exceptions occurring during assembly scanning will be re-thrown. assembly scanning exceptions can be ignored using the following:

snippet:SwallowScanningExceptions

WARNING: Ignoring assembly scanning exceptions can cause the endpoint to not load some features, behaviors, messages or message handlers and behave incorrectly.
