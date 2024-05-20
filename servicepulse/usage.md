---
title: Usage
summary: Viewing endpoint usage summary and generating a usage report
component: ServicePulse
reviewed: 2024-05-08
related:
---

Endpoint usage summary and report generation are available in the Usage page of ServicePulse.
This is where a usage report can be generated from at license renewal time.

> [!NOTE]
> The usage functionality requires ServicePulse version 1.39 or later, and ServiceControl version 5.3 or later.

## Viewing

At any time the system usage can be viewed on the Usage page. It is divided into two sections:

- detected endpoints
- detected broker queues

For each endpoint and queue, the maximum daily throughput is displayed. This can be helpful in seeing how the system is growing over time, and hence getting an understanding of which [tier](https://particular.net/pricing) the endpoint belongs to for licensing purposes.

### Detected endpoints

Detected endpoints are those identified by the system as NServiceBus endpoints and are included in usage report for NServiceBus licensing purposes. There is an option to set the endpoint type if there is a valid reason as to why it should not be included in the licensing calculations. Any changes made to the endpoint type are automaically saved.

### Detected broker queues

If the system is using an [NServiceBus transport](./../transports) that allows querying of metrics, then any queues detected on the broker that cannot be automatically identified as NServiceBus endpoints will be listed in the `Detected Broker Queues` tab. These queues will be included in the usage report for NServiceBus licensing purposes. There is an option to set the endpoint type if there is a valid reason as to why a queue should not be included in the licensing calculations. Any changes made to the endpoint type are automaically saved.

### Bulk endpoint updates

If multiple endpoints or queues matching a certain naming pattern need to be set to a certain endpoint type, then this can be done in bulk using the filter option.

1. Find the endpoints/queues that need to be bulk updated

TODO: image

2. Press the `Set displayed Endpoint Types to` button to select which endpoint type the filtered results should be set to.

TODO: image

All endpoints/queues on screen will be updated to the selected endpoint type and automatically saved.

## Generating a usage report

Clicking `Generate Report` generates a usage report file with the detected endpoints and queues. The report includes the endpoint type selections made on screen, and any specified [words to mask](usage-config.md#report-masks) with be obfuscated.

The report **will not** be automatically uploaded or sent to Particular - it is a JSON file that needs to be provided to Particular upon request.
