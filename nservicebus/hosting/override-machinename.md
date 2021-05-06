---
title: Overriding the host identifier
summary: How to override the endpoint host identifier
component: Core
versions: '[4.0,)'
redirects:
 - nservicebus/override-hostid
reviewed: 2020-04-07
---


## Host identifier

In NServiceBus, all messages sent to the audit queue include two extra headers: `$.diagnostics.hostid` and `$.diagnostics.hostdisplayname`. These extra headers uniquely identify the running host for the endpoint, i.e. the operating system host (not to be confused with `NServiceBus.Host`). 

The host ID is used by ServiceControl to map a running endpoint to the host where they are deployed. This information is then displayed in ServicePulse and ServiceInsight so it's possible to identify in which host the endpoint is running.

The default values in most scenarios is the machine name for `$.diagnostics.hostdisplayname` and a hash of the running executable's  path concatenated with the machine name for `$.diagnostics.hostid`. If CloudServices host is used in an Azure deployment, NServiceBus uses [role name and instance ID](/nservicebus/hosting/cloud-services-host/faq.md#host-identifier) instead.


## Overriding the host identifier

There are scenarios where the rules used by NServiceBus to generate a `hostid` and `hostdisplayname` are not adequate and the user needs to take control, e.g. in environments where endpoint upgrades are done to a new path or when self-hosting in Azure deployments.

Manual configuration is required when deployments may end up in different paths than previously deployed versions (e.g. using [Octopus Deploy](https://octopus.com/)). The `hostid` must remain the same across restarts unless the physical host has changed.

snippet: HostIdFixer


## Stable host identifiers when using Docker

Docker containers hosted in Kubernetes pose unique challenges to creating a stable host identifier, as a [Deployment](https://kubernetes.io/docs/concepts/workloads/controllers/deployment/) will not have a stable, unique identifier. This results in the endpoint having a new, random identity in ServicePulse and ServiceInsight every time the Pod containing the endpoint is (re)scheduled on a different node.

Instead of a Deployment, an endpoint can be deployed to Kubernetes using a [StatefulSet](https://kubernetes.io/docs/concepts/workloads/controllers/statefulset/), which provides stable, unique network identifiers, allowing the host identifier to be specified as shown above.
