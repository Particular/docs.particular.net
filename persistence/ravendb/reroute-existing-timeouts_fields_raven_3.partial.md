* Update the following values in the Timeout Data documents in Raven Persistence with the new endpoint name or updated machine name:
  * Destination.Queue (contains endpoint name only)
  * Destination.Machine (contains only machine name)
  * OwningTimeoutManager (contains endpoint name only)
  * Headers:
    * NServiceBus.Timeout.RouteExpiredTimeoutTo
    * NServiceBus.OriginatingEndpoint 
* After the above changes, the endpoint needs to be restarted to process the timeout.
