---
title: Platform Sample Installation
summary: Installation instructions for using the platform sample for demo purposes.
component: PlatformSample
reviewed: 2019-06-16
---

The [Platform Sample package](https://www.nuget.org/packages/Particular.PlatformSample) is a portable version of the Particular Service Platform which can be used to demonstrate the platform without installing it. This is the easiest way to experience the Particular Service Platform. It is also possible to include and show the platform in demos or samples.

WARNING: The Platform Sample is not for production usage and should only be used for demonstrating it during presentations or in samples.

The Platform Sample package includes ServiceControl, ServiceControl.Monitoring, and ServicePulse. All are configured to use the [Learning Transport](/transports/learning/) and don't support any other transports.

## Installation

From within an existing Visual Studio solution, the following steps need to be executed:

1. Add a new Console Application
1. Add a reference to the `Particular.PlatformSample` NuGet package to the new Console Application.
1. Add the following code:

```cs
static void Main(string[] args)
{
    Particular.PlatformLauncher.Launch();
}
```

The Platform Sample package will perform the following actions upon execution:

1. Copy platform binaries into the project output directory.
1. Find available ports for all the platform tools.
1. Launch ServiceControl and ServiceControl.Monitoring
1. Wait for the ServiceControl API to be responsive, which can take a while
1. Open a browser window with ServicePulse.

## Options

The `PlatformLauncher.Launch()` API has very limited options available as optional parameters, which can be mixed as needed.

### Showing console output

By default the console outputs of ServiceControl and ServiceControl.Monitoring are suppressed. To view them for purposes of debugging or curiosity, use the `showPlatformToolConsoleOutput` parameter:

```cs
Particular.PlatformLauncher.Launch(showPlatformToolConsoleOutput: true);
```

### ServicePulse default route

Some samples benefit from opening ServicePulse to a specific view, rather than the Dashboard.

For example, to open ServicePulse to the Monitoring view:

```CSharp
Particular.PlatformLauncher.Launch(servicePulseDefaultRoute: "/monitoring");
```

## Sample

The [Message Replay Tutorial](/tutorials/message-replay) is a sample that includes the Platform Sample and is used to view and retry failed messages. It can be used to verify how the Platform Sample works from within an existing Visual Studio solution.

To have ServiceInsight use the sample, [download](https://github.com/Particular/serviceinsight/releases/latest) and install ServiceInsight. Then connect to `http://localhost:49200/api` from the [Endpoint Explorer](/serviceinsight/#endpoint-explorer) to connect to the running Service Control.

Note: The Platform Sample package will search for available ports and will display the ports used. The ServiceControl port needs to be used for the endpoint explorer to connect to the right ServiceControl instance.
