#### Propagating the tenant information downstream

In order to propagate the tenant information to the outgoing messages (including timeouts) this sample uses the same approach as the [tenant information propagation sample](/samples/multi-tenant/propagation/): a pair of behaviors, one in the incoming pipeline and the other in the outgoing pipeline.