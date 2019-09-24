---
title: Platform Sample installation
summary: Installation instructions for using the platform sample for demo purposes.
component: PlatformSample
reviewed: 2019-07-06
---

The [Platform Sample package](https://www.nuget.org/packages/Particular.PlatformSample) is a portable version of the Particular Service Platform which can be used to demonstrate the platform without installing it. This is the easiest way to experience the Particular Service Platform. It is also possible to include and show the platform in demos or samples.

WARNING: The Platform Sample is not for production usage and should only be used for demonstrating the Platform during presentations or in samples.

The Platform Sample package includes instances of ServiceControl, ServiceControl Monitoring, and ServicePulse. They are all configured to use the [Learning Transport](/transports/learning/). Other transports are not supported by the Platform Sample package. At this time, ServiceInsight is not included in the Platform Sample package.

## Installation

In existing Visual Studio solution, the following steps are required:

1. Add a new Console App project.
1. Install the `Particular.PlatformSample` NuGet package.
1. Add the following code to `Program.Main`:

snippet: Launch

The Platform Sample package performs the following when the Console App is started:

1. Copies platform binaries into the project output directory.
1. Finds available ports for all the platform tools.
1. Launches ServiceControl and ServiceControl.Monitoring.
1. Waits for the ServiceControl API to be responsive, which can take a while.
1. Opens a browser window with ServicePulse.

## Options

`PlatformLauncher.Launch()` accepts some optional parameters.

### Showing console output

By default, the console outputs of ServiceControl and ServiceControl. onitoring are suppressed. To view them for debugging, or curiosity, specify:

snippet: ShowConsoleOutput

### ServicePulse default route

Some samples benefit from opening ServicePulse with a specific view, rather than the dashboard.

For example, to open ServicePulse with the Monitoring view:

snippet: ServicePulseDefaultRoute

## Sample

The [message replay tutorial](/tutorials/message-replay) is a sample that uses the Platform Sample package to view and retry failed messages. It shows how the Platform Sample package works from within an existing Visual Studio solution.

To use ServiceInsight with the sample, [download](https://github.com/Particular/serviceinsight/releases/latest) and install ServiceInsight. After starting ServiceInsight, connect to ServiceControl using the [Endpoint Explorer](/serviceinsight/#endpoint-explorer) at, for example, `http://localhost:49200/api`.

Note: The Platform Sample package will search for available ports and then display the ports being used. The actual ServiceControl port that needs to be used in the Endpoint Explorer can be seen in this output.