---
title: Message Throughput Throttling
reviewed: 2017-07-28
component: Core
tags:
- Throttling
related:
- nservicebus/pipeline/manipulate-with-behaviors
- nservicebus/operations/tuning
---

This sample demonstrates performing a search query against the [GitHub API](https://developer.github.com/v3/) using the [Octokit library](https://github.com/octokit/octokit.net). It runs in unauthenticated mode, which is limited to a fixed number of requests per minute. Upon hitting this limit, the endpoint will delay processing additional messages, until the limit resets.

The solution consists of two endpoints; Sender and Limited. The reason two multiple are required in this scenario is are that NServiceBus does not support limiting messages by message type. So to limit **only** a specific message type then an endpoint is required to handle that message type.


## Sender

The Sender is a normal endpoint that sends the `SearchGitHub` message and the handles the reply `SearchResponse` message.


### Sending

The message sending occurs at startup.

snippet: Sending


### Search Response

Handling the reply.

snippet: GitHubSearchResponseHandler


## Limited

This endpoint is limited to 1 concurrent message processing and uses a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) to handle when the processing limit of Octokit has been exceeded.


### Configure to use 1 concurrent process

[Limits the endpoint concurrency](/nservicebus/operations/tuning.md).

snippet: LimitConcurrency


### Search Handler

Performs the Octokit search.

snippet: SearchHandler


### Registering the behavior in pipeline

snippet: RegisterBehavior

snippet: RegisterStep


### The Behavior

Handles detection of `Octokit.RateLimitExceededException` and defers the message.

snippet: ThrottlingBehavior
