---
title: Installers
summary: Installers ensure endpoint specific artifacts are installed and configured automatically.
reviewed: 2016-03-17
component: core
tags:
 - Installation
redirects:
 - nservicebus/nservicebus-installers
---

NServiceBus has the concept of installers to make sure that endpoint specific artifacts e.g., queues, directories, or databases are installed and configured automatically, if needed, at install time.

To create a custom installer is as easy as implementing the `INeedToInstallSomething` interface.

snippet:InstallSomething

NServiceBus scans the assemblies in the runtime directory for installers so no code is needed to register them.


Installers can be enabled to run at startup:

snippet:Installers
