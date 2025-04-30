## Remote processor 

The transactional session supports configuring a remote endpoint as the processing endpoint. The key use case for this is to allow the transactional session to be used in [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting) endpoints.

snippet: configure-remote-processor

### Outbox cleanup

For persisters where [Outbox cleanup](/nservicebus/outbox/#outbox-expiration-duration) is performed by the endpoint only the remote processing endpoint will have the cleanup disabled to prevent concurrent cleanup from happening.
