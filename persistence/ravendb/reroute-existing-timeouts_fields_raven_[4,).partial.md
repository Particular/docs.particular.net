* Update the following values in the Timeout Data documents in the Raven persister with the new endpoint name or updated machine name:
  * Destination
  * OwningTimeoutManager (contains endpoint name only)
  * Headers:
    * NServiceBus.ReplyToAddress
    * NServiceBus.Timeout.RouteExpiredTimeoutTo
    * NServiceBus.OriginatingEndpoint
* After the above changes are made, restart the endpoint to process the timeout.
