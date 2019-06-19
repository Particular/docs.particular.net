---
title: Platform Sample Installation
summary: Installation instructions for using the platform sample for demo purposes.
component: PlatformSample
reviewed: 2019-06-16
---

The Platform Sample can be used to demonstrate the platform without actually fully installing it. This is the easiest way to get some experience to learn the Particular Service Platform. It is also possible to include and show the platform in presentations or samples. It is also possible to (temporarily) include it in a custom Visual Studio solution for demonstration purposes.

DANGER: The Platform Sample is not for production usage and should only be used for demonstrating it during presentations or in samples.

The Platform Sample package includes the binaries of ServiceControl and ServiceControl.Monitoring and a self-hosted version of ServicePulse. All are configured to use the LearningTransport and don't support any other transports.

## Sample

The [Message Replay Tutorial](/tutorials/message-replay) is a sample that includes the Platform Sample and is used to view and retry failed messages. It can be used to verify how the Platform Sample works from within an existing Visual Studio solution.

To have ServiceInsight use the sample, [download](https://github.com/Particular/serviceinsight/releases/latest) and install ServiceInsight. Then connect to `http://localhost:49200/api` from the [Endpoint Explorer](/serviceinsight#endpoint-explorer) to connect to the running Service Control.

Note: If port `49200` is in use by another process, the Platform Sample console application will start by outputting the used port.

## Installation

Within an existing Visual Studio solution a new console application needs to be added. Within that project, the following steps need to be executed:

1. Add a reference to the `Particular.PlatformSample` NuGet package.
1. Add the following code

```CSharp
static void Main(string[] args)
{
    Particular.PlatformLauncher.Launch();
}
```

The package will perform the following actions upon execution:

1. Copy platform binaries into the project output directory.
1. Find available ports for all the platform tools.
1. Find .learningtransport directory and folders for logging.
1. Launch ServiceControl with modified config file.
1. Launch ServiceControl.Monitoring with modified config file.
1. Serve the ServiceControl web assets using the Kestrel HTTP server.
1. Wait for the ServiceControl API to be responsive.
1. Open a browser window pointing to the ServicePulse UI.

## Options

The `PlatformLauncher.Launch()` API has very limited options available as optional parameters, which can be mixed as needed.

### Showing console output

By default the console outputs of ServiceControl and ServiceControl.Monitoring are suppressed. To view them for purposes of debugging or curiosity, use the `showPlatformToolConsoleOutput` parameter:

```CSharp
Particular.PlatformLauncher.Launch(showPlatformToolConsoleOutput: true);
```

### ServicePulse default route

Some samples benefit from opening ServicePulse to a specific view, rather than the Dashboard.

For example, to open ServicePulse to the Monitoring view:

```CSharp
Particular.PlatformLauncher.Launch(servicePulseDefaultRoute: "/monitoring");
```
