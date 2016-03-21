---
title: Throughput Throttling
summary: Control the throttling of messages.
reviewed: 2016-03-21
tags: 
- Throttling
related:
- samples/throttling
- nservicebus/pipeline/customizing-v6
---

This sample demonstrates performing a search query against [GitHub API](https://developer.github.com/v3/) using the [Octokit library](https://github.com/octokit/octokit.net) in unauthenticated mode, which is limited to a fixed number of requests per minute. Upon hitting this limit, the endpoint will delay processing additional messages, until the limit resets.


### Configure to use 1 concurrent process

snippet:Configuration


### Registering the behavior in pipeline

snippet:RegisterBehavior


### The Behavior

snippet: ThrottlingBehavior