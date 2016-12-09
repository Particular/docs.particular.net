---
title: Installers
summary: Installers ensure endpoint specific artifacts are installed and configured during endpoint startup.
reviewed: 2016-12-09
component: core
tags:
 - Installation
redirects:
 - nservicebus/nservicebus-installers
---

Installers ensure that endpoint specific artifacts (e.g. queues, directories, databases etc) are installed and configured at endpoint startup install time.

To create a custom installer implement the `INeedToInstallSomething` interface.

snippet:InstallSomething

Assemblies in the runtime directory are scanned for installers so no code is needed to register them.


Installers can be enabled to run at startup:

snippet:Installers
