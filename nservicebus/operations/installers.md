---
title: Installers
summary: Installers ensure endpoint specific artifacts are installed and configured during endpoint startup.
reviewed: 2017-08-17
component: core
related:
 - nservicebus/operations
tags:
 - Installation
redirects:
 - nservicebus/nservicebus-installers
---

Installers ensure that endpoint specific artifacts (e.g. queues, directories, databases etc) are installed and configured at endpoint startup install time.

## Running installers

partial: default-behavior

Installers can be enabled to always run at startup:

snippet: Installers

Installers may need to be run depending on the arguments that are provided to the host or aspects the environment the endpoint is hosted in.

For example Installers can be enabled based on the command line arguments provided:

snippet: InstallersRunWhenNecessaryCommandLine

or by a machine name convention like:

snippet: InstallersRunWhenNecessaryMachineNameConvention

partial: disable


## Custom installers

To create a custom installer implement the `INeedToInstallSomething` interface.

snippet: InstallSomething

Assemblies in the runtime directory are scanned for installers so no code is needed to register them.

