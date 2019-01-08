---
title: "Monitoring NServiceBus Demo - Setup instructions"
reviewed: 2019-01-08
summary: Detailed instructions for setting up the NServiceBus monitoring demo
---

This document explains how to set up the [NServiceBus monitoring demo](/tutorials/monitoring-demo/) in detail.

## Prerequisites

In order to run the demo your system needs to meet the following requirements:

- Windows 8 or higher
- .NET Framework 4.6.1 and higher

## Running the sample

The sample zip file includes 4 endpoints and the Particular Software Platform components, all of which have been configured to talk to each other using the Learning Transport.

Once you have downloaded the zip package make sure to **unblock** it in the file properties, if needed:

![Unblock the package](unblock-demo-package.png "width=401")

After unblocking the zip file, extract its contents into a folder. For the rest of this tutorial, we will refer to this folder as `MonitoringDemo`.

Open the `MonitoringDemo` folder and double-click on `MonitoringDemo.exe`. This executable will:

1. Run the platform components .
    - A ServiceControl instance (binds to a free port in the 49200 range)
    - A Monitoring instance (binds to a free port in the 49200 range)
2. Run the sample endpoints, each in their own window
3. Run ServicePulse, in it's own window (binds to a free port in the 49200 range)
4. Open your default browser to the ServicePulse monitoring tab.
5. Allow to scale out or scale in the sales endpoint by pressing the `O` or `I` key.
6. Wait for ENTER key, and will quit all processes and cleanup temporary files and folders.

## Explore the demo

Once you have the demo up and running, [begin exploring the demo](/tutorials/monitoring-demo/#demo-walk-through).
