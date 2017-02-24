
### The Saga

The saga shown in the sample is a very simple order management saga that:

 * handles the creation of an order;
 * offloads the payment process to a different handler;
 * handles the completion of the payment process;
 * completes the order;