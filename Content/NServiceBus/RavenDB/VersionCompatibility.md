---
title: RavenDB Version Compatibility 
summary: The various approaches used by different versions of NServiceBus when integrating with RavenDB
tags:
- RavenDB
- Persistence
---

## ILMerged into the Core 

In **version 3 of NSerivceBus** the default persistence was changed from NHibernate to RavenDB. The required RavenDB assemblies were [ILMerged](http://research.microsoft.com/en-us/people/mbarnett/ilmerge.aspx) into NServiceBus.Core.dll to give users a seamless OOTB experience.

This worked but had several negative side effects

 * The RavenDB classes had to be internalized to avoid namespace conflicts when people also reference the actual Raven assemblies. This meant a strong typed configuration API, that takes a `DocumentStore`, was not possible.
 * If consumers of upgraded to newer versions of Raven assemblies, for bug fixes or performance improvements, it was not possible for NServiceBus to leverage these newer assemblies. NServiceBus was hard coded to use the ILMerged versions.
 * Any changes in the compatibility of the Raven Client and Server would require a new version of NServiceBus be release with a new ILMerged version of Raven.

### ILMerged RavenDB client versions 

* Versions 3.0-3.2 of NServiceBus were shipped with version 1.0.616 of RavenDB ilmerged
* Version 3.3 of NServiceBus shipped with version 1.0.992 of RavenDB ilmerged

## Resource Merged into the core

In **version 4 of NServiceBus** the approach to embedding Raven in NServiceBus.Core.dll changed from ILMerge to resource merging 

This allowed us, at runtime, to chose the newest version of the Raven assemblies found on disk. So if a consumer of NServiceBus has updated to newer raven assemblies NServiceBus would use those instead of the merged versions. 

This resolved all the issue with ILMerged but raised a different one:  **Compatibility between different versions of the Raven client assemblies**. NServiceBus need to use a small subset of the Raven client APIs. At any one time we need to choose one version of those APIs to reference. This means that any incompatibilities between different versions of the Raven client API require a new version of NServiceBus to be release that copes with that incompatibility using reflection.  

### Resource merged RavenDB versions 

Version 4 of NServiceBus was shipped with version 1.0.616 of RavenDB resource merged

## Externalised RavenDB

Available on nuget as [NServiceBus.RavenDB](https://www.nuget.org/packages/NServiceBus.RavenDB)

    Install-Package NServiceBus.RavenDB

The current approach moving forward will be to change the RavenDB integration to ship outside the core in a stand alone assembly NServiceBus.RavenDB.dll. This has the following advantages

 * Allow us to evolve the implementation more closely instep with the RavenDB release schedule. 
 * Reduce the need for version compatibility hacks
 * Allow the shipping upgrades to this library without having to ship the core. 
 * Make the approach to RavenDB persistence consistence with the other persistence integrations 

### Supported RavenDB versions 

 * RavenDB Client 2.5.2908. Note that if there are breaking changes in the RavenDB Client this may change in future versions of this library. However this will be handled explicitly through a dependency in the nuget package  
 * RavenDB Server 2.5.2908 

### Supported NServiceBus "Core" versions

 * Version 1 of NServiceBus.RavenDB will be targeting NServiceBus 4.1 and higher
 * Version 2, and higher, of NServiceBus.RavenDB will be targeting NServiceBus 5.0 and higher. 
 