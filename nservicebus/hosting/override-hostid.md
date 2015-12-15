---
title: Override host identifier
summary: How to override the endpoint host identifier
tags: []
redirects:
 - nservicebus/override-hostid
---

## What is a host identifier

Since NServiceBus Version 4.4, all messages sent to the audit queue include two extra headers, these are `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`. These extra headers uniquely identify the running host (not to be confused with `NServiceBus.Host`, in this scenario host refers to the operating system host) for the endpoint. The defaults are the machine name for `$.diagnostics.hostdisplayname` and for `$.diagnostics.hostid` is a hash of the running executable installed path concatenated with the machine name. These defaults mostly work, except in environments where endpoint upgrades are done to a new path or in Azure deployments.


## What is a host identifier used for

The host identifier is used by ServiceControl to map a running endpoint to the host where they are deployed. This information is then displayed in ServicePulse so that OPS knows the "host" that an endpoint is deployed to, so they can quick diagnose issues with the host environment.


## When do I need to override a host identifier

As described above, there are scenarios where the default rules used to generate a `hostid` and `hostdisplayname` are not adequate and the user needs to take control. In an Azure deployment the NServiceBus framework takes care of updating these defaults for the user automatically, so in Azure the default are role name for `$.diagnostics.hostdisplayname` and for `$.diagnostics.hostid` is the role InstanceId.

Another deployment where it is needed for the user to manage these settings is when using [Octopus Deploy](https://octopusdeploy.com/). Octopus Deploy creates a new folder for each deployment, these being upgrades or new deployments, because of that, the user needs to customize the `hostid` so that the id the same across endpoint restarts unless physical host has changed. 


## How do I override an endpoint host identifier

snippet:HostIdFixer
