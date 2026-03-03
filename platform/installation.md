---
title: Platform Installation
summary: Guidance on the different ways to deploy and use the platform
reviewed: 2024-06-25
---

The Particular Service Platform may be installed on either a server, for use in production environments, or a workstation, during development. A [portable version](platform-sample-package.md) is also available, for use in samples or demonstrations.

Note that, for the purposes of this document, shared testing environments may be treated in the same way as production environments.

## Installation

Platform components are deployed separately, and the technique used varies depending on the component being deployed.

### NServiceBus

NServiceBus is a NuGet package that is used by the development team to create endpoints that are [hosted](/nservicebus/hosting/) in nearly any .NET compatible process. For instance:

- Hosted in a console application:
  - Running in a terminal or as a batch job
  - Running as a Windows service
  - Running as a Linux daemon, in or out of a container
- Hosted in an ASP.NET or similar web application
- Hosted as a function, e.g. Azure Functions / AWS Lambda
- Hosted in a WPF or similar Windows application

As a result, NServiceBus endpoints are deployed using whatever tools are applicable to the hosting model being used.

### ServiceControl

ServiceControl is deployed as [instances of different types](/servicecontrol/#servicecontrol-instance-types). Each instance type can be deployed using:

#### ServiceControl Management utility (Windows only)

ServiceControl Management utility is a Windows application which can be found under the ServiceControl section on the [downloads page](https://particular.net/downloads).

This application is used to deploy and manage one or more instances of any of the ServiceControl instance types as Windows services.

#### PowerShell (Windows only)

A PowerShell module is provided to deploy any of the ServiceControl instance types as a Windows service.

#### Containers (Linux only)

Container images are pushed to [Docker Hub](https://hub.docker.com/u/particular) creating the ability to deploy any of the ServiceControl instance types on a Linux and [OCI compatible](https://opencontainers.org/) container host, e.g. Docker or Kubernetes.

#### Platform Sample NuGet package

ServiceControl, along with ServicePulse, can be hosted in Visual Studio, with no installation required, using the [Platform Sample package](/platform/platform-sample-package.md). This is useful for presentations, for providing samples, or to demonstrate usage of the Platform within an existing solution.

### ServicePulse

ServicePulse is a single page application which can be deployed in multiple ways:

#### Integrated in ServiceControl

ServiceControl version 6.13 and above includes [integrated ServicePulse](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) and can host ServicePulse from a ServiceControl [Error instance](/servicecontrol/servicecontrol-instances/) host.

#### Separate from ServiceControl

ServicePulse can be installed as a separate application from ServiceControl, although it still must be connected to a ServiceControl Error instance to function.

##### Windows service

ServicePulse can be installed using a dedicated [installation package](https://particular.net/downloads), which deploys ServicePulse as a Windows service host.

ServicePulse can be installed more than once on a single machine, with each instance listening on its own port. This is done by [specifying appropriate arguments during installation](/servicepulse/installation.md#installation-available-installation-parameters).

##### Container (Linux only)

A container image for ServicePulse is pushed to [Docker Hub](https://hub.docker.com/u/particular) creating the ability to deploy ServicePulse on a Linux and [OCI compatible](https://opencontainers.org/) container host, e.g. Docker or Kubernetes.

##### Extracted

Using the [installation package](https://particular.net/downloads), ServicePulse can be [extracted as a set of HTML, JavaScript, and CSS files](/servicepulse/install-servicepulse-in-iis.md#basic-setup-detailed-steps) and subsequently deployed to any web server.

#### Platform Sample NuGet package

ServicePulse, along with ServiceControl, can be hosted in Visual Studio, with no installation required, using the [Platform Sample package](/platform/platform-sample-package.md). This is useful for presentations, for providing samples, or to demonstrate usage of the Platform within an existing solution.

#### Switch between ServiceControl Error instances

When multiple ServiceControl Error instances are deployed, ServicePulse can be configured to switch between instances via the [ServicePulse UI connection management screen](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui).
