Once orders are successfully stored in the system, they need to be processed, paid, and shipped. This requires orchestrating multiple activities, some of which may not be under our direct control. Clearly, we don’t want to deliver orders that haven’t been paid, or, even worse, charge the customer and ship nothing.

Traditional database transactions fall short in such scenarios—they cannot help coordinate activities when external resources, like payment gateways or shipping couriers with HTTP APIs, are involved. These external systems cannot be enlisted in database transactions, yet we still need to ensure that business processes like “ship only when paid” are handled correctly and reliably.

This is where PostgreSQL as both a queue and persistence shines, especially in systems built with NServiceBus. By combining PostgreSQL’s robust transactional capabilities with messaging patterns, we can design distributed, eventually consistent systems without introducing unnecessary complexity.

![](https://particular.net/images/solutions/retail/saga-diagram-horizontal.png)

For instance, we can use NServiceBus sagas—message-driven state machines—to model long-running business transactions that provide consistency across asynchronous events. Using PostgreSQL as the data store for both messaging and persistence, we can coordinate activities like payment and shipping, ensuring that orders are shipped only after payment confirmation.

Here’s how it works:
1. A saga is initiated when an event announces that an order has been submitted, with the order details stored in PostgreSQL.
1. The saga listens for subsequent events, such as payment confirmation, to ensure the order is paid before proceeding.
1. Once the payment event arrives, the saga triggers a shipping process by placing a message in a PostgreSQL-backed queue, ensuring that the order is handed off for delivery.

But what if the payment confirmation never arrives? That’s no problem. Sagas allow you to model time within your business processes. By setting a timeout message to arrive in the future, you can check if payment has been received within a specific timeframe. If not, compensating actions—like canceling the order or notifying the customer—can be initiated.

## Why PostgreSQL?

PostgreSQL enables this flow by serving as both the queue for messaging and the persistence store for order and saga data, ensuring:
- No Additional Infrastructure: There’s no need for a separate message broker or outbox pattern—PostgreSQL handles both messaging and persistence in one system.
- Atomicity: A single transaction handles database updates and message dispatch, reducing the risk of inconsistency.
- Durability: All messages and business data are reliably stored in one system, simplifying operations.
- Scalability: PostgreSQL’s proven scalability ensures it can handle both high-throughput messaging and growing business data.

By leveraging PostgreSQL in this dual role, you can build robust, distributed systems that simplify the orchestration of long-running business processes without sacrificing consistency or reliability.
