---
title: RavenDB Version Compatibility
summary: The various approaches used by different versions of NServiceBus when integrating with RavenDB
tags:
- Persistence
reviewed: 2017-10-16
redirects:
 - nservicebus/ravendb/version-compatibility
---

include: dtc-warning

## Externalized RavenDB

RavenDB persistence is available as a separate NuGet [NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB)

The current approach moving forward for the RavenDB integration is to ship outside the core in a stand alone assembly NServiceBus.RavenDB.dll. This has the following advantages

 * Enables evolving the implementation more closely instep with the RavenDB release schedule;
 * Reduce the need for version compatibility hacks;
 * Allows the shipping of upgrades to this library without having to ship the core;
 * Makes the approach to RavenDB persistence consistent with the other persistences;


### Supported RavenDB versions

 * Check the minimum supported version on the [NuGet site](https://www.nuget.org/packages/NServiceBus.RavenDB).


### Supported NServiceBus versions

 * Version 1 of NServiceBus.RavenDB targets NServiceBus 4.1 through 4.x;
 * Version 2 & 3 of NServiceBus.RavenDB target NServiceBus 5.x;
 * Version 4 of NServiceBus.RavenDB targets NServiceBus 6.x;


## NServiceBus 4: Resource Merged into the core

In Version 4 of NServiceBus the approach to embedding RavenDB in NServiceBus.Core.dll changed from [ILMerge](https://github.com/Microsoft/ILMerge) to resource merging.

This enables, at runtime, to chose the newest version of the RavenDB assemblies found on disk. So if a consumer of NServiceBus has updated to newer RavenDB assemblies NServiceBus would use those instead of the merged versions.

This resolved all the issue with ILMerged but raised a different one:  **Compatibility between different versions of the RavenDB client assemblies**. NServiceBus need to use a small subset of the RavenDB client APIs. At any one time it is necessary to choose one version of those APIs to reference. This means that any incompatibilities between different versions of the RavenDB client API require a new version of NServiceBus to be released that copes with that incompatibility using reflection.

The root underlying cause of these compatibility issue is that NServiceBus follows SemVer but RavenDB doesn't.


### Resource merged RavenDB versions

Version 4 of NServiceBus was shipped with Version 2.0.2375 of RavenDB resource merged.


### NServiceBus 4 and RavenDB 2.5

NServiceBus Version 4 uses RavenDB.Client Version 2.0 internally. Using RavenDB client 2.0 against a RavenDB Server 2.5 is not recommended. Server restarts may result in wiping out outstanding transactions potentially resulting in message loss. Therefore if using RavenDB server Version 2.5, do the following:

1. Install version 1 of NServiceBus.RavenDB NuGet package in the endpoint. This will ensure that RavenDB Client Version 2.5 is being used instead of 2.0
```ps
Install-Package NServiceBus.RavenDB -version 1.X.Y
```
NOTE: Replace `X.Y` with latest minor/patch version.
1. Then use the new initialization extension methods to properly setup persistence for RavenDB 2.5.

RavenDB integration, in both the core and the externalized versions is configured using extension methods. Since these cannot be made distinct using namespace or type,  re-using the same extension method names would result in type conflicts. To avoid the conflicts the externalized version has had to slightly rename the extension points.

Previously:

snippet: OldRavenDBPersistenceInitialization

Now:

snippet: Version2_5RavenDBPersistenceInitialization


## NServiceBus 3: ILMerged into the Core

In NServiceBus Version 3 the default persistence was changed from NHibernate to RavenDB. The required RavenDB assemblies were [ILMerged](https://github.com/Microsoft/ILMerge) into NServiceBus.Core.dll to give users a seamless OOTB experience.


### ILMerged RavenDB client versions

 * Versions 3.0-3.2 of NServiceBus were shipped with version 1.0.616 of RavenDB ILMerged.
 * Version 3.3 of NServiceBus shipped with version 1.0.992 of RavenDB ILMerged.
