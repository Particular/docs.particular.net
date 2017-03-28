
## The Saga

The saga shown in the sample is a very simple order management saga that:

 * Handles the creation of an order.
 * Offloads the payment process to a different handler.
 * Handles the completion of the payment process.
 * Completes the order.