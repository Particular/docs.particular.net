---
title: Overriding the host identifier
summary: How to override the endpoint host identifier
component: Core
versions: '[4.0,)'
redirects:
 - nservicebus/override-hostid
reviewed: 2016-10-19
---


## Host identifier

In NServiceBus, all messages sent to the audit queue include two extra headers, these are `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`. These extra headers uniquely identify the running host for the endpoint, i.e. the operating system host (not to be confused with `NServiceBus.Host`). 

The host ID is used by ServiceControl to map a running endpoint to the host where they are deployed. This information is then displayed in ServicePulse and ServiceInsight so it's possible to identify on which host the endpoint is running.

The default values in most scenarios is the machine name for `$.diagnostics.hostdisplayname` and a hash of the running executable's  path concatenated with the machine name for `$.diagnostics.hostid`. If CloudServices host is used in the Azure deployment, then NServiceBus uses [role name and instance ID](/nservicebus/hosting/cloud-services-host/faq.md#host-identifier) instead.


## Overriding the host identifier

There are scenarios where the rules used by NServiceBus to generate a `hostid` and `hostdisplayname` are not adequate and the user needs to take control, i.e. in environments where endpoint upgrades are done to a new path or when self-hosting in Azure deployments.

Manual configuration is required when deployments may end up in different paths than previously deployed versions (e.g. using [Octopus Deploy](https://octopus.com/)). The `hostid` needs to remain the same across restarts unless the physical host has changed.

snippet: HostIdFixer
