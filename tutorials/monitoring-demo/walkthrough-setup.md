---
title: "Monitoring NServiceBus Demo - Setup instructions"
reviewed: 2024-01-16
summary: Detailed instructions for setting up the NServiceBus monitoring demo
---

This document explains how to set up the [NServiceBus monitoring demo](/tutorials/monitoring-demo/).

## Prerequisites

To run the demo, your system must meet the following requirements:

- .NET 8 or higher

## Running the demo

The sample zip file includes 4 endpoints and the Particular Software Platform components, all of which have been configured to send messages using the [Learning Transport](/transports/learning).

After downloading the zip file, extract its contents into a folder. Open the folder and double-click on `MonitoringDemo`. This executable will:

1. Run the platform components.
    - A ServiceControl instance (binds to a free port in the 49200 range)
    - A Monitoring instance (binds to a free port in the 49200 range)
2. Run the sample endpoints, each in their own window.
3. Run ServicePulse, in its own window (binds to a free port in the 49200 range).
4. Open your default browser to the ServicePulse monitoring tab.
5. Allow the sales endpoint to scale out or scale in by pressing the <kbd>&uarr;</kbd> or <kbd>&darr;</kbd> keys.
6. Pressing the <kbd>enter</kbd> key will quit all processes and cleanup temporary files and folders.

## Explore the demo

Once you have the demo up and running, [begin exploring the demo](/tutorials/monitoring-demo/#demo-overview).
