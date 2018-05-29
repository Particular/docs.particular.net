
To ensure correctness, when creating new sagas, the persister applies [optimistic concurrency](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) using an incrementing counter. The persister uses an explicit version column combined with the `ReadComitted` isolation level.

However, to avoid excessive conflicts for highly congested sagas, the persister uses pessimistic concurrency for querying and updating saga information. The pessimistic concurrency is implemented using a `SELECT ... FOR UPDATE` construct or its dialect-specific equivalent.