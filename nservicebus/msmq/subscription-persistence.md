---
title: MSMQ Subscription Persistence
component: Core
reviewed: 2016-08-25
---

WARNING: Storing subscriptions in MSMQ is not suitable when endpoints are scaled-out, because the subscription queue cannot be shared among multiple endpoints.

partial: config


partial: timeouts