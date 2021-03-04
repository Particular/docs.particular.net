By default, RavenDB persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data, though starting with NServiceBus.RavenDB version 6.5, it's possible to configure the persister to use pessimistic locking.
See [later in this document](saga-concurrency.md#sagas-concurrency-control) for how to do this.
