
## Tuning send performance outside the scope of a handler

Only settings at the ASB SDK level impact this scenario, more specifically

  * `MessagingFactories().BatchFlushInterval()`: Controls the time that the ASB SDK batches client side requests before sending them out. A value of TimeSpan.Zero turns this feature off. 
  * `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()`: Each factory manages a tcp connection to a front end node in the broker, this connection has throughput limits. Opening more factories significantly improves send performance.
  * `NumberOfClientsPerEntity`: Ensure there is 1 send client per factory.

The way the send api is invoked matters a lot as well. Using `await` on each `Send()` operation will make the ASB SDK wait until the send operation completes and therefor makes client side batching useless. It's better to maintain a list of send tasks and await them all together instead.

Slow:

Snippet: asb-slow-send

Fast:

Snippet: asb-fast-send