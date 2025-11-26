---
title: Platform Sample installation
summary: Installation instructions for using the platform sample for demo purposes.
component: PlatformSample
reviewed: 2024-11-06
---

The [Platform Sample package](https://www.nuget.org/packages/Particular.PlatformSample) is a portable version of the Particular Service Platform which can be used to demonstrate the platform without installing it. This is the easiest way to experience the Particular Service Platform.

> [!WARNING]
> The Platform Sample is not suited for production usage and should only be used for demonstrating the Platform during presentations or in samples.

The Platform Sample package includes instances of ServiceControl, ServiceControl Monitoring, and ServicePulse. They are all configured to use the [Learning Transport](/transports/learning/). Other transports are not supported by the Platform Sample package.

## Installation

In any existing Visual Studio solution, the following steps are required:

1. Add a new Console App project.
1. Install the `Particular.PlatformSample` NuGet package.
1. Add the following code to `Program.Main`:

snippet: Launch

The Platform Sample package performs the following when the Console App is started:

1. Copies platform binaries into the project output directory.
1. Finds available ports for all the platform tools.
1. Launches ServiceControl and ServiceControl.Monitoring.
1. Waits for the ServiceControl API to be responsive, which may take a while.
1. Opens a browser window with ServicePulse.

## Options

`PlatformLauncher.Launch()` accepts some optional parameters.

### Showing console output

By default, the console output from ServiceControl and ServiceControl.Monitoring is suppressed. For debugging or curiosity, console output may be shown by specifying:

snippet: ShowConsoleOutput

### ServicePulse default route

Some samples benefit from opening ServicePulse with a specific view, rather than the dashboard.

For example, to open ServicePulse with the Monitoring view:

snippet: ServicePulseDefaultRoute

## Sample

The [message replay tutorial](/tutorials/message-replay) is a sample that uses the Platform Sample package to view and retry failed messages. It shows how the Platform Sample package works from within an existing Visual Studio solution.

> [!NOTE]
> The Platform Sample package will search for available ports and then display the ports being used. The actual ServiceControl port that needs to be used in the Endpoint Explorer can be seen in the output.
