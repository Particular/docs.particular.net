* In Raven Persistence in Timeout Data document the following values need to be updated to contain new endpoint name or updated machine name:
  * Destination.Queue (contains endpoint name only)
  * Destination.Machine (contains only machine name)
  * OwningTimeoutManager (contains endpoint name only)
  * Headers:
    * NServiceBus.Timeout.RouteExpiredTimeoutTo
    * NServiceBus.OriginatingEndpoint 
* After the above change the endpoint need to be restarted to process the timeout
