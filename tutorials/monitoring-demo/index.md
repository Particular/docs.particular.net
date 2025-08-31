---
title: "NServiceBus monitoring demo"
reviewed: 2025-08-22
summary: A self-contained demo solution to explore the monitoring features of the Particular Service Platform.
suppressRelated: true
redirects:
- tutorials/monitoring/demo
---

Experience the monitoring features of the Particular Service Platform by running a real-world demo in ServicePulse. This downloadable sample includes all required platform components, pre-configured and ready to use, with four sample endpoints that communicate by exchanging messages.

<div class="text-center inline-download hidden-xs">
  <a id='download-demo' href='https://s3.amazonaws.com/particular.downloads/MonitoringDemo/Particular.MonitoringDemo.zip' class="btn btn-primary btn-lg">
    <span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span> Download demo
  </a>
</div>

## Prerequisites

To run the sample, ensure you have:

- [.NET 8 runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed
- Windows operating system:
  - Desktop: Windows 8 or later
  - Server: Windows Server 2016 or later

> [!NOTE]
> While this specific sample requires Windows, the Particular Service Platform does not.

## How to run the sample

1. Download and extract the zip package.
2. Open the extracted folder.
3. Double-click `MonitoringDemo` to start the demo.

For more details on the demo setup, see the [setup walkthrough](walkthrough-setup.md).

## Demo overview

When running, the demo starts four endpoints configured as shown below:

![Solution Diagram showing four endpoints](diagram.svg "width=680")

By default, the ClientUI endpoint sends one `PlaceOrder` message per second.

All endpoints are configured to send monitoring data to the Particular Service Platform, which you can view in ServicePulse.

![ServicePulse monitoring tab showing sample endpoints](servicepulse-monitoring-tab-sample-low-throughput.png "width=500")

## Explore the demo further

Use the monitoring tools in ServicePulse to investigate:

- **[Which message types take the longest to process?](walkthrough-1.md)**
  Analyze individual endpoint performance to identify optimization opportunities.
- **[Which endpoints have the most work to do?](walkthrough-2.md)**
  Detect traffic peaks to make informed scaling decisions.
- **[Are any of the endpoints struggling?](walkthrough-3.md)**
  Uncover and resolve hidden issues before they cause message processing failures.

include: monitoring-demo-next-steps
