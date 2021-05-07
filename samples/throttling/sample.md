---
title: Message Throughput Throttling
reviewed: 2021-04-17
component: Core
related:
- nservicebus/pipeline/manipulate-with-behaviors
- nservicebus/operations/tuning
---

Systems often need to integrate with 3rd party services, some of which may limit the number of concurrent requests they process.

This sample demonstrates an integration with the [GitHub API](https://developer.github.com/v3/) using the [Octokit library](https://github.com/octokit/octokit.net). It runs in unauthenticated mode, which is limited to a fixed number of requests per minute. Upon hitting this limit, the endpoint will delay processing additional messages, until the limit resets.

The solution consists of two endpoints; Sender and Limited. The reason two endpoints are required in this scenario is that NServiceBus does not support limiting messages by message type. So, to limit **only** a specific message type, a separate endpoint is used for it.


## Sender

The Sender is a normal endpoint that sends the `SearchGitHub` message and the handles the reply `SearchResponse` message.


### Sending

The message sending occurs at startup.

snippet: Sending


### Handling the response

Handling the GitHubSearchResponse message.

snippet: GitHubSearchResponseHandler


## Limited

This endpoint is limited to processing one message at a time and uses a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) to handle when the processing limit of Octokit has been exceeded.


### Configure to process only one concurrent message

[Limits the endpoint concurrency](/nservicebus/operations/tuning.md).

snippet: LimitConcurrency


### Search Handler

Performs the Octokit search.

snippet: SearchHandler


### Registering the behavior in pipeline

snippet: RegisterBehavior

### The pipeline behavior

Handles the detection of `Octokit.RateLimitExceededException` and defers the message.

snippet: ThrottlingBehavior

NOTE: The behavior sends a *copy* of the original message, but does not copy the headers of the original message. If the headers of the original message are required, they must be copied from `context.Headers` to `SendOptions`.
