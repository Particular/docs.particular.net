---
title: NServiceBus Installers
summary: Installers ensure endpoint specific artifacts are installed and configured automatically.
reviewed: 2016-03-17
tags:
- Installation
redirects:
- nservicebus/nservicebus-installers
---

NServiceBus has the concept of installers to make sure that endpoint specific specific artifacts e.g., queues, folders, or databases are installed and configured automatically, if needed, at install time.

To create a custom installer is as easy as implementing the `INeedToInstallSomething` interface.

snippet:InstallSomething

NServiceBus scans the assemblies in the runtime directory for installers so no code is needed to register them.

**Version 3.0 Only:** Version 3.0 included an interface called `INeedToInstallInfrastructure<T>` interface. It was primarily used for things that are not specific to a given endpoint e.g., RavenDB or MSMQ. This interface has been obsoleted in Version 4.0 and will be removed in 5.0, since the introduction of [PowerShell commandlets](management-using-powershell.md) to aid the installation of infrastructure.

When are they invoked?

When using the NServiceBus host, installers are invoked as shown:

| Command Line Parameters          | Infrastructure (v3.0 Only) Installers | Regular Installers
|----------------------------------|:-------------------------------------:|:------------------:
| /install NServiceBus.Production  | &#10004;                              | &#10004;
| NServiceBus.Production           | &#10006;                              | &#10006;
| /install NServiceBus.Integration | &#10004;                              | &#10004;
|  NServiceBus.Integration         | &#10006;                              | &#10004;
| /install NServiceBus.Lite        | &#10004;                              | &#10004;
| NServiceBus.Lite                 | &#10006;                              | &#10004;

The installers are controlled by both the `/install` command line option to the host and the current profile in use. Custom profiles can be created to meet other specific requirements.

When self hosting NServiceBus, invoke the installers manually, using this:

snippet:Installers

NOTE: The use of `/installInfrastructure` flag with the `NServiceBus.Host` has been deprecated in Version 4.0. To install needed infrastructure, use the [PowerShell commandlets](management-using-powershell.md) instead.