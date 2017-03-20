
## Routing

By default a request is routed to the local endpoint instance. It is possible to override the routing behavior with:

snippet: WcfRouting

the replying endpoint handler:

snippet: WcfReplyFromAnotherEndpoint

The delegate is invoked for each service type discovered. The delegate needs to return a function delegate which creates a `SendOption` instance every time it is called.