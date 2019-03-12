## Running the project

 1. Start the Sender project (right-click on the project, select the `Debug > Start new instance` option).
 1. The text `Press <enter> to send a message` should be displayed in the Sender's console window.
 1. Start the Receiver project (right-click on the project, select the `Debug > Start new instance` option).
 1. The Sender should display subscription confirmation `Subscribe from Receiver on message type OrderSubmitted`.
 1. Press `A` or `B` on the Sender console to send a new message either to one of the tenants.


## Verifying that the sample works correctly

 1. The Receiver displays information that an order was submitted.
 1. The Sender displays information that the order was accepted.
 1. Finally, after a couple of seconds, the Receiver displays confirmation that the timeout message has been received.
 1. Open SQL Server Management Studio and go to the tenant databases. Verify that there are rows in saga state table (`dbo.OrderLifecycleSagaData`) and in the orders table (`dbo.Orders`) for each message sent.

WARNING: If used with a message transport that does not support native timeouts, timeout data is stored in a shared database so make sure to not include any sensitive information. Keep such information in saga data and only use timeouts as notifications.
