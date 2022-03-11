When deferring, the message will have similar headers compared to a _send_ with a few differences:

 * The message will have `IsDeferredMessage` with a value of `true`.
 * Since the defer feature uses the timeouts feature, the timeout headers will exist.
 * The `Timeout.RouteExpiredTimeoutTo` header contains the queue name for where the callback for the timeout should be sent.
 * The `Timeout.Expire` header contains the timestamp for when the timeout should fire.
