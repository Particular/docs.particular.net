#if-version [,8)
> [!NOTE]
> The approach used in this sample can mitigate some of the architectural drawbacks of the [NServiceBus Scheduler](/nservicebus/scheduling/). The NServiceBus scheduler is built on top of the [Timeout Manager](/nservicebus/messaging/timeout-manager.md) which leverages the queuing system to trigger scheduled actions. Under heavy load there may be some disparity between the expected time of a scheduled action and execution time due to the delay between timeout messages being generated and processed.
#end-if