---
title: NServiceBus Installers
summary: Installers ensure endpoint specific artifacts are installed and configured automatically.
reviewed: 2016-03-17
tags:
 - Installation
redirects:
 - nservicebus/nservicebus-installers
related:
 - nservicebus/hosting/nservicebus-host
---

NServiceBus has the concept of installers to make sure that endpoint specific artifacts e.g., queues, folders, or databases are installed and configured automatically, if needed, at install time.

To create a custom installer is as easy as implementing the `INeedToInstallSomething` interface.

snippet:InstallSomething

NServiceBus scans the assemblies in the runtime directory for installers so no code is needed to register them.

**Version 3.0 Only:** Version 3.0 included an interface called `INeedToInstallInfrastructure<T>` interface. It was primarily used for things that are not specific to a given endpoint e.g., RavenDB or MSMQ. This interface has been obsoleted in Version 4.0 and will be removed in 5.0, since the introduction of [PowerShell commandlets](management-using-powershell.md) to aid the installation of infrastructure.

Installers can be enabled to run at startup:

snippet:Installers
