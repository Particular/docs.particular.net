 * `EndpointMySql`, `EndpointSqlServer`, and `EndpointOracle` projects act as "servers" to run the saga instance.
 * Receive the `StartOrder` message and initiate a `OrderSaga`.
 * `OrderSaga` requests a timeout with a `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.