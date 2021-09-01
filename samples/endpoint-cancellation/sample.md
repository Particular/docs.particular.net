---
title: Endpoint cancellation
summary: Stopping an endpoint with a cancellation token
reviewed: 2021-08-31
component: Core
---


Run the solution. One console application starts up, `Server`.


## Code walk-through

When the endpoint is started a `CancellationToken` is passed in from a `CancellationTokenSource`

snippet: StartingEndpointWithCancellationToken

A message is sent to the endpoint once the endpoint starts which triggers a long running message handler:

snippet: LongRunningMessageHandler

Once a key is pressed the `CancellationTokenSource.Cancel` method is called to signal the cancellation token, stop the long running handler, and shut down the endpoint.

snippet: StoppingEndpointWithCancellationToken