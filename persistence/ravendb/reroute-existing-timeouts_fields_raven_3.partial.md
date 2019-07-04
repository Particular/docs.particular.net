* Update the following values in the Timeout Data documents in the Raven persister with the new endpoint name or updated machine name:
  * Destination.Queue (contains endpoint name only)
  * Destination.Machine (contains only machine name)
  * OwningTimeoutManager (contains endpoint name only)
  * Headers:
    * NServiceBus.Timeout.RouteExpiredTimeoutTo
    * NServiceBus.OriginatingEndpoint 
* After the above changes are made, restart the endpoint to process the timeout.
