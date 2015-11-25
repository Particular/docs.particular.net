---
title: NServiceBus Installers
summary: Installers ensure endpoint specific artifacts are installed and configured automatically.
tags:
- Installation
redirects:
- nservicebus/nservicebus-installers
---

NServiceBus has the concept of installers to make sure that endpoint specific specific artefacts e.g., queues, folders, or databases are installed and configured automatically for you if needed at install time.

To create your own installer is as easy as implementing the `INeedToInstallSomething<T>` interface. 

#### Version 3 and 4 only

    public interface INeedToInstallSomething<T> : INeedToInstallSomething where T : IEnvironment
    {
        void Install(string identity);
    }


#### All versions

The generic parameter gives you a way to restrict your installer to a specific platform. Currently this is either Windows or Azure.

If you don't care about the runtime environment, just use the `INeedToInstallSomething` interface instead.

    public interface INeedToInstallSomething
    {
        void Install(string identity);
    }

NServiceBus scans the assemblies in the runtime directory for installers so you don't need any code to register them.

**Version 3.0 Only:** Version 3.0 included an interface called `INeedToInstallInfrastructure<T>` interface. It was primarily used for things that are not specific to a given endpoint e.g., RavenDB or MSMQ. This interface has been obsoleted in version 4.0 and will be removed in 5.0, since the introduction of [PowerShell commandlets](management-using-powershell.md) to aid the installation of infrastructure.

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

The installers are controlled by both the `/install` command line option to the host and the current profile in use. You can, of course, implement your own profile if you have other requirements.

When self hosting NServiceBus, invoke the installers manually, using this:

snippet:Installers

NOTE: The use of `/installInfrastructure` flag with the `NServiceBus.Host` has been deprecated in version 4.0. To install needed infrastructure, use the [PowerShell commandlets](management-using-powershell.md) instead.

