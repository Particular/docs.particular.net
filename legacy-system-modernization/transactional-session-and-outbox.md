---
title: Ensuring data integrity
summary: Modernizing legacy systems and maintaining data integrity
reviewed: 2025-04-07
---

When modifying business data and sending messages in ASP.NET applications, achieving data integrity and consistency isn't trivial.

In a modern, message-driven system, NServiceBus provides features such as the [transactional session](/nservicebus/transactional-session/) and the [outbox](/nservicebus/outbox/) to maintain consistency without requiring MSDTC, supporting exactly-once processing within a local transaction scope.
