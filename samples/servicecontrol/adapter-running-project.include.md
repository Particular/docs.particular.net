## Running the project

 1. Start the projects: Adapter, Sales and Shipping. Ensure the adapter starts first because on start-up it creates a queue that is used for heartbeats.
 1. Open ServicePulse (by default it's available at `http://localhost:9090/#/dashboard`) and select the Endpoints Overview. The Shipping endpoint should be visible in the Active Endpoints tab as it has the Heartbeats plugin installed.
 1. Go to the Sales console and press `o` to create an order.
 1. Notice the Shipping endpoint receives the `OrderAccepted` event from Sales and publishes `OrderShipped` event.
 1. Notice the Sales endpoint logs that it processed the `OrderShipped` event.
 1. Go to the Sales console and press `f` to simulate message processing failure.
 1. Press `o` to create another order. Notice the `OrderShipped` event fails processing in Sales and is moved to the error queue.
 1. Press `f` again to disable message processing failure simulation in Sales.
 1. Go to the Shipping console and press `f` to simulate message processing failure.
 1. Go back to Sales and press `o` to create yet another order. Notice the `OrderAccepted` event fails in Shipping and is moved to the error queue.
 1. Press `f` again to disable message processing failure simulation in Shipping.
 1. Open ServicePulse and select the Failed Messages view.
 1. Notice the existence of one failed message group with two messages. Open the group.
 1. One of the messages is `OrderAccepted` which failed in `Shipping`, the other is `OrderShipped` which failed in `Sales`.
 1. Press the "Retry all" button.
 1. Go to the Shipping console and verify that the `OrderAccepted` event has been successfully processed.
 1. Go to the Sales console and verify that both `OrderShipped` events have been successfully processed.
 1. Shut down the Shipping endpoint.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on the that icon to open the Endpoints Overview. Notice that the Shipping is now displayed in the Inactive Endpoints tab.