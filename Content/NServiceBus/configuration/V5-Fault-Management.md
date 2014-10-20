---
title: Configuration API Fault Management in V5
summary: Configuration API Fault Management
 in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

It is possible to inject a custom retry policy, for Second Level Retries, via the `CustomRetryPolicy` method exposed by the `SecondLevelRetries` configuration setting. A custom retry policy allows to have fine control over the retry logic for each failing message.