---
title: Installers
summary: Installers ensure endpoint-specific artifacts are installed and configured during endpoint startup.
reviewed: 2020-05-20
component: core
related:
 - nservicebus/operations
redirects:
 - nservicebus/nservicebus-installers
---

Installers ensure that endpoint-specific artifacts (e.g. database tables, queues, directories, etc.) are created and configured.

## When to run installers

Installers require permissions to administer resources such as database tables, queues, or directories. Following the [principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege), it is recommended to run an endpoint with these elevated permissions only during initial deployment.

For example, an endpoint can be started manually as an administrator or elevated user to allow installers to run. The required resources will be ready as soon as the endpoint has successfully started. The endpoint can then be shut down and installed as a service, which can run as a regular user that does not have permission to modify resources.

The alternative to using installers is to create the required resources before the endpoint is run. The method of doing this varies for each transport or persistence package. For more information, see [operations](/nservicebus/operations).

## Running installers

partial: default-behavior

Installers can be enabled to always run at startup:

snippet: Installers

Installers may need to be run depending on the arguments that are provided to the host or aspects the environment the endpoint is hosted in.

For example, installers can be enabled based on command line arguments:

snippet: InstallersRunWhenNecessaryCommandLine

They can also be enabled by a machine name convention:

snippet: InstallersRunWhenNecessaryMachineNameConvention

partial: disable

NOTE: Some combinations of transports / persisters / DI containers may require `.Start` to be called instead of `.Create`. In this case call both `.Start` and `.Stop` and allow the endpoint to shutdown immediately after startup.

## Custom installers

Implement the `INeedToInstallSomething` interface to create a custom installer:

snippet: InstallSomething

Assemblies in the runtime directory are scanned for installers so no code is needed to register them.
