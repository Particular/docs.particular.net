---
title: Accessing data via NHibernate persistence
summary: How to access business data using connections managed by the NServiceBus NHibernate persistence.
component: NHibernate
versions: '[4,]'
reviewed: 2020-04-07
related:
 - nservicebus/handlers/accessing-data
redirects:
 - nservicebus/nhibernate/accessing-data
---

NHibernate persistence supports a mechanism that allows using the same data context used by NServiceBus internals to also store business data. This ensures atomicity of changes done across multiple handlers and sagas involved in processing of the same message. See [accessing data](/nservicebus/handlers/accessing-data.md) to learn more about other ways of accessing the data in the handlers.

partial: direct-access

Regardless of how the `ISession` object is accessed, it is fully managed by NServiceBus according to the best practices defined by NServiceBus documentation with regards to transactions.

partial: customizing-session

partial: limitations

partial: transactionscope