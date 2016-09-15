NOTE: For debugging purposes, it can be helpful to increase the `RequestedHeartbeat` and `DequeueTimeout` settings as shown below:

snippet:rabbitmq-connectionstring-debug

Increasing these settings can help prevent the connection to the broker from timing out while the code is paused from hitting a breakpoint.