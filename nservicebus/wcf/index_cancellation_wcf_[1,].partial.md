
## Cancellation

By default a request is canceled after 60 seconds. It is possible to override the cancellation behavior with:

snippet:WcfCancelRequest

The delegate is invoked for each service type discovered. The delegate needs to return a time span indicating how long a request can take until it gets canceled.