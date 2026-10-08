---
title: Audited/Failed Message Display and Discovery
summary: Describes how ServicePulse displays and allows filtering for audited and failed messages
component: ServicePulse
reviewed: 2026-09-10
related:
- servicepulse/intro-failed-messages
- servicepulse/message-details
- servicepulse/platform-health
---

## Overview

ServicePulse provides visibility into the flow of messages through the system, including both audited and failed messages. It displays the message status, message type, message ID, processing time, critical time, delivery time, and time sent in a list view. Clicking any message navigates to a detailed view of that message.

![All Messages](images/all-messages.png 'width=800')

> [!NOTE]
> The time range picker, search history, query cancellation, and the other query bar features described on this page require ServicePulse version 2.12 or later.

## Message Information

![All Message Info](images/all-messages-info.png 'width=800')

Each row in the list represents a message in the system. Each message is prefixed with a status icon that shows the nature of the message.


| Status Icon | Description |
|------------|-------------|
| ![Success Message](images/success-message-icon.png 'width=30')| Shows when a message was processed successfully on the first attempt|
| ![Resolved Successfully Message](images/resolved-successfully-message-icon.png 'width=30')|Shows when a message succeeded after retries|
| ![Failed Message](images/failed-message-icon.png 'width=30')| Shows when a message has failed|
| ![Archived Message](images/archived-message-icon.png 'width=30')|Shows when a failed message has been deleted|
| ![Repeated Failed Message](images/repeated-failed-message-icon.png 'width=30')| Shows when a message has failed multiple times|
| ![Retry Message](images/retry-issued-message-icon.png 'width=30')| Shows when a retry has been requested for a failed message|

A warning symbol <img src="images/warning-icon.png" width="20" alt="warning"> also appears when any of these conditions are met:

- When a message needed retries to succeed
- When any of the timing metrics (critical, processing, or delivery time) have negative values, which could indicate timing issues or clock synchronization problems

The following time-related information about the message is also displayed:

- **[Processing Time](/monitoring/metrics/definitions.md#metrics-captured-processing-time):** The time taken by the receiving endpoint to successfully process an incoming message.
- **[Critical Time](/monitoring/metrics/definitions.md#metrics-captured-critical-time):** The total duration from when a message is sent to when it is fully processed.
- **Delivery Time:** The time taken to deliver the message from sender to receiver before processing begins.
- **Time Sent:** The timestamp when the message was originally sent from the sending endpoint, followed by how long ago that was (for example, `9:25:24 AM · 4 minutes ago`).

The format of **Time Sent** adapts to the age of the message: only the time for messages sent today, `yesterday` or the weekday name for messages sent within the past week, and the full date beyond that. Hovering over the timestamp shows it in full in both local time and UTC. The **Times** option on the results line switches every timestamp in the list between local time and UTC; the choice is remembered per browser.

## Filtering

The results can be filtered by one or more of the following criteria:

![All Messages query bar](images/all-messages-filter.png 'width=800')

- **Search:** Perform a free-text search across message data. See [filtering options](#filtering-options) for the supported syntax.
- **Endpoint:** Select a specific endpoint.
- **Sent:** Limit the results to messages sent within a time range. See [time range](#filtering-time-range).

Every change to a filter runs the query immediately; there is no separate search button. The filters are part of the page URL, so a link to the view reproduces the same query.

By default, the view displays 100 messages but can be customized to display 50, 250, or 500 messages using the **Show** option on the results line. To display specific messages, modify the filters to narrow down the displayed results.

### Time range

By default, the view shows messages sent in the last six hours. Querying a bounded range keeps the view responsive on large audit databases, where an unbounded query can take many seconds.

![Time range picker](images/all-messages-time-range.png 'width=600')

Clicking the **Sent** value opens the time range picker, which offers:

- **Quick ranges:** Last 15 minutes, last hour, last 6 hours, last 24 hours, last 7 days, today, yesterday, and this week. **No time filter** removes the time range entirely, which queries the whole audit retention window.
- **Absolute time range:** A start and an end, each entered as text. A calendar button next to each field fills in the day; the time stays as typed.

Each field accepts:

- A relative expression such as `now-6h`, `now-1d/d` (the start of yesterday), or `now/w` (the start of this week). Relative ranges keep sliding as time passes, so a range from `now-6h` to `now` always covers the most recent six hours, including under auto-refresh.
- An absolute timestamp in the form `YYYY-MM-DD HH:mm[:ss]`. A timestamp without a zone is read as local time. Append `Z` for UTC or an offset such as `+02:00`.
- A pasted ISO 8601 interval such as `2026-09-01T08:00:00Z/2026-09-01T12:00:00Z`, which fills both fields at once. This is convenient when copying timestamps from log files.

The picker also allows saving the current range as the default for this browser, so the view opens on that range instead of the last six hours.

### Search history

Queries that used search text or an endpoint are remembered per browser, up to the ten most recent. When the search field has focus, the recent searches appear below it, each with the search text, endpoint, and time range it ran with. Typing narrows the list, the arrow keys move through it, and selecting an entry runs that exact query again. Pressing <kbd>Escape</kbd> or moving away from the field closes the list.

![Search history](images/all-messages-history.png 'width=500')

## Filtering Options

The search filter works in the following way:

- The filter is case insensitive (i.e., `term` and `TERM` will return the same results).
- A message will be returned if it matches at least one of the terms (i.e., the logical operator between the terms is `OR`).
- A `*` wildcard can be used to replace a prefix and/or a postfix of a searched term (i.e., `word` will be found by `*rd`, `wo*`, and `*or*`).
- Other logical operators (e.g., conjunction, negation) are not supported.


> [!NOTE]
> A message's body is searchable only if the body size is under 85kB, within the [`ServiceControl.Audit/MaxBodySizeToStore` size limit](/servicecontrol/audit-instances/configuration.md#performance-tuning-servicecontrol-auditmaxbodysizetostore), and is a non-binary content type.

## Sorting Options

![Results line with Show, Sort, and Times](images/all-messages-sort.png 'width=500')

Messages can be sorted by any of the following criteria, using the **Sort** option on the results line:

- Latest sent
- Oldest sent
- Slowest processing time
- Highest critical time
- Longest delivery time

## Refresh Messages

![Refresh button and auto-refresh interval](images/all-messages-refresh.png 'width=300')

The view supports both manual and automatic refresh. These options update the displayed information with the latest updates from the ServiceControl database.

- **Manual Refresh:** Click the **Refresh** button to run the current query again.
- **Auto-Refresh:** The segment next to the refresh button shows the auto-refresh interval and opens a menu to change it. While auto-refresh is active, a ring inside the refresh button counts down to the next refresh. The available intervals are:
  - Off
  - Every 5 seconds
  - Every 15 seconds
  - Every 30 seconds
  - Every minute
  - Every 10 minutes
  - Every 30 minutes
  - Every hour

When a refresh brings in messages that were not in the list before, the new rows are highlighted briefly as they arrive.

> [!NOTE]
> Having a low auto-refresh interval continually active can have a negative impact on ServiceControl's performance.

## Slow and failed queries

Large audit databases can make a query take several seconds. The view stays usable while a query runs: the existing results remain visible, the filters can be changed, and the refresh button turns into **Cancel**, which stops the running query on the ServiceControl instance as well. Changing a filter while a query is still running cancels that query and runs the new one.

The results line reports what the last query cost, for example `Showing 100 of 158,736,340 result(s) · took 2.7 s · ran 3 minutes ago`. When a query has been running for more than five seconds, the results line suggests narrowing the time range, which is the most effective way to make a query lighter.

If a query fails, or exceeds the query time limit that ServiceControl version 6.20 and later enforce, the view explains what happened and offers the next narrower time ranges as one-click buttons.

### Partial results

When ServiceControl gathers results from several audit instances and one of them does not answer in time, the view shows the results that did arrive together with a warning naming each instance that contributed nothing and why (timed out, unreachable, or returned an error). The total is then presented as a lower bound, for example `Showing 100 of at least 87,421,337 result(s)`. The warning links to [Platform Health](/servicepulse/platform-health.md) to check the listed instances.

> [!NOTE]
> Partial results require ServiceControl version 6.20 or later. With older versions, a query that any audit instance fails to answer in time fails as a whole.
