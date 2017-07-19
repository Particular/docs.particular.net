### Toggle script creation

At compile time the SQL persistence validates that [sagas are of the correct base type and match the required conventions](/persistence/sql/saga.md). This can be problematic when mixing persistences. For example using the SQL persistence for timeouts but another persister for saga storage. As such it is possible to selectively control what SQL scripts are generated at compile time, and as a side effect avoid the convention validation of sagas.


#### Disable outbox script creation

snippet: ProduceOutboxScripts


#### Disable saga script creation

snippet: ProduceSagaScripts


#### Disable subscriptions script creation

snippet: ProduceSubscriptionScripts


#### Disable timeout script creation

snippet: ProduceTimeoutScripts