---
title: Usage
summary: Viewing endpoint usage summary and generating a usage report
component: ServicePulse
reviewed: 2024-05-08
related:
  - nservicebus/throughput-tool
---

This section allows for viewing the endpoint usage summary of a system using NServiceBus.
A usage report can be generated here at license renewal time.

> [!NOTE]
> The usage data collection functionality requires ServicePulse version 1.39 or later, and ServiceControl version 5.3 or later.

> [!NOTE]
> If no data is available, go to the [Usage Setup](usage-config.md) screen to diagnose connection problems.

## Generating a usage report

Clicking `Generate Report` generates a usage report file with the detected [endpoints](#viewing-usage-summary-detected-endpoints) and [queues](#viewing-usage-summary-detected-broker-queues). The report includes the [endpoint type](#viewing-usage-summary-setting-an-endpoint-type) selections made on screen, and any specified [words to mask](usage-config.md#report-masks) with be obfuscated.

The report **will not** be automatically uploaded or sent to Particular - it is a JSON file that needs to be provided to Particular upon request.

## Viewing usage summary

At any time the system usage can be viewed on the Usage page. It is divided into two sections:

- [detected endpoints](#viewing-usage-summary-detected-endpoints)
- [detected broker queues](#viewing-usage-summary-detected-broker-queues)

For each endpoint and queue, the maximum daily throughput is displayed. This can be helpful in seeing how the system is growing over time, and hence getting an understanding of which [tier](https://particular.net/pricing) the endpoint belongs to for licensing purposes.

### Detected endpoints

Detected endpoints are those identified by the system as NServiceBus endpoints and are included in usage report for NServiceBus licensing purposes. There is an option to set the endpoint type if there is a valid reason as to why it should not be included in the licensing calculations. Any changes made to the endpoint type are automaically saved.

### Detected broker queues

If the system is using an [NServiceBus transport](./../transports) that allows querying of metrics, then any queues detected on the broker that cannot be automatically identified as NServiceBus endpoints will be listed in the `Detected Broker Queues` tab. These queues will be included in the usage report for NServiceBus licensing purposes.

### Setting an endpoint type

The usage summary may contain detected queues that should not be counted as part of a license with Particular Software. The raw report data will be processed by a tool that will ignore throughput caused, for example, by ServiceControl and related tools that should not be counted for licensing purposes.

In addition, the detected endpoints and queues screen provides an option to set the endpoint type which provides a reason as to why a queue should not be included in the licensing calculations. Any changes made to the endpoint type are automaically saved.

#### Bulk endpoint updates

If multiple endpoints or queues matching a certain naming pattern need to be set to an endpoint type, then this can be done in bulk using the filter option.

1. Find the endpoints/queues that need to be bulk updated

TODO: image

2. Press the `Set displayed Endpoint Types to` button to select which endpoint type the filtered results should be set to.

TODO: image

All endpoints/queues on screen will be updated to the selected endpoint type and automatically saved.