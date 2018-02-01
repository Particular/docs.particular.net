## Running the project

 1. Start the Adapter, Sales and Shipping projects.
 1. Open ServicePulse (by default it's available at `http://localhost:9090/#/dashboard`) and select the Endpoints Overview. The Shipping endpoint should be visible in the Active Endpoints tab as it has the Heartbeats plugin installed.
 1. Go to the Sales console and press `o` to send a message.
 1. Notice the Sales endpoint receives its own message and successfully processes it.
 1. Press `f` to simulate message processing failure.
 1. Go to the Shipping console and also press `f` to simulate message processing failure.
 1. Press `o` in Sales to create more messages.
 1. Notice both messages failed processing in their respective endpoints.
 1. Open ServicePulse and select the Failed Messages view.
 1. Notice the existence of one failed message group with two messages. Open the group.
 1. Press the "Retry all" button.
 1. Go to the Shipping console and verify that the message has been successfully processed.
 1. Go to the Sales console and verify that the message has been successfully processed.
 1. Shut down the Shipping endpoint.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on the that icon to open the Endpoints Overview. Notice that the Shipping endpoint is now displayed in the Inactive Endpoints tab.
