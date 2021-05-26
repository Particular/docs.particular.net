---
title: ServiceInsight Application Invocation
reviewed: 2020-03-16
component: ServiceInsight
summary: How the integration between ServicePulse and ServiceInsight works and how to use the parameterized invocation of ServiceInsight.
---

It is possible to load any given message in ServiceInsight from ServicePulse by clicking the "View in ServiceInsight" link.

![ServicePulse Error Messages](images/007-servicepulse-error-messages.png 'width=500')

The benefits of this feature can be leveraged without installing ServicePulse.

Click the link in ServicePulse, it opens a link in the browser:

```
si://localhost:33333/api?search=487b5055-11bb-4a70-a4fd-a2c00125aa43
```

This is the URI to which ServiceInsight listens. It launches ServiceInsight and does the following:

 * Automatically connects to localhost on port 33333
 * Performs a search on a specific Message ID

This means that a specific message that was in the error queue will be displayed as soon ServiceInsight opens, allowing the cause to be seen.

The URI can be generated manually as long as the convention is followed. Construct the URI and send it to the development team, who can then paste it into the Run window or a browser.

A list of the supported parameters and a description of their effects:

 - `Search` [String]: Performs a search using the full-text search feature of ServiceInsight. This can be a message identifier, a part of the message payload, or any value in the message header.
  - Example: `si://localhost:33333/api?search=SubmitOrder`
 - `EndpointName` [String]: The name of the endpoint to select upon startup. When an endpoint is selected, the operations and message lists are filtered to the endpoint.
  - Example: `si://localhost:33333/api?EndpointName=VideoStore.Sales&search=SubmitOrder`
 - `AutoRefresh` [Integer]: Turns on the `AutoRefresh` option so that the view is refreshed, based on the value provided here (in seconds).
  - Example: `si://localhost:33333/api?EndpointName=VideoStore.Sales&search=SubmitOrder&Auto&AutoRefresh=5`
 - `SilentStartup` [Boolean]: When set to true, the splash screen will not show when launching the application.

When ServiceInsight is launched using these parameters, notice the effects on the user interface (having the endpoint selected, message selected, etc.). If a passed-in argument is not supported, ServiceInsight will notify when it runs.