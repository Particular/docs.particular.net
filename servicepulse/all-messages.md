---
title: Audited/Failed Message Display and Discovery
summary: Describes how ServicePulse displays and allows filtering for audited and failed messages
component: ServicePulse
reviewed: 2025-05-11
related:
- servicepulse/intro-failed-messages
- servicepulse/message-details
---

## Overview

ServicePulse provides visibility into the flow of messages through the system, including both audited and failed messages. It displays the message status, message type, message ID, processing time, critical time, time sent, and time delivered in a comprehensive list view. Clicking any message navigates to a detailed view of that message.

![All Messages](images/all-messages.png 'width=800')

> [!NOTE]
> Currently, ServicePulse can be connected to only one ServiceControl instance at a time. To view the message list for other ServiceControl instances, go to the [connections in the configuration tab](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) to set up the URL.

## Message Information

![All Message Info](images/all-messages-info.png 'width=800')

Each row in the list represents a message in the system. Successfully processed messages are prefixed with <img src="images/success-message-icon.png" width="20" alt="successful message">, and failed messages are prefixed with <img src="images/failed-message-icon.png" width="20" alt="failed message">. The following time-related information about the message is also displayed:

- **[Processing Time](/monitoring/metrics/definitions.md#metrics-captured-processing-time):** The time taken by the receiving endpoint to successfully process an incoming message.
- **[Critical Time](/monitoring/metrics/definitions.md#metrics-captured-critical-time):** The total duration from when a message is sent to when it is fully processed.
- **Time Sent:** The timestamp when the message was originally sent from the sending endpoint.
- **Delivery Time:** The time taken to deliver the message from sender to receiver before processing begins.


## Filtering

The results can be filtered by one or more of the following criteria:

![All Message Info](images/all-messages-filter.png 'width=800')

- **Date Range:** Specify the date range along with the time within that range.
- **Endpoint:** Select a specific endpoint.
- **Custom Filter:** Perform a free-text search across message data. This also supports wildcards.

By default, the view displays 100 messages but can be customized to display up to a maximum of 500 messages. To display specific messages, modify the filters to narrow down the displayed results.

> [!NOTE]
> A message's body is searchable only if the body size is under 85kB, within the `ServiceControl.Audit/MaxBodySizeToStore` size limit, and is a non-binary content type.

## Sorting Options

![All Message Sort](images/all-messages-sort.png 'width=200')

Messages can be sorted by any of the following criteria:

- Latest Sent
- Oldest Sent
- Slowest Processing Time
- Highest Critical Time
- Longest Delivery Time

## Refresh Messages

![All Message Info](images/all-messages-refresh.png 'width=200')

The view supports both manual and automatic refresh. These options update the displayed information with the latest updates from the ServiceControl database.

- **Manual Refresh:** Click the "Refresh List" button to manually refresh the list.
- **Auto-Refresh:** Automatically refreshes the list at configurable intervals, delivering near real-time information. The available intervals are:
  - Every 5 seconds
  - Every 15 seconds
  - Every 30 seconds
  - Every 1 minute
  - Every 10 minutes
  - Every 30 minutes
  - Every 1 hour
  
  > [!NOTE]
  > Having a low auto-refresh time continually active can have a negative impact on ServiceControl's performance
