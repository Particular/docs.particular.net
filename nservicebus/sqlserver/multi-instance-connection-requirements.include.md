In [*multi-catalog* and *multi-instance* modes](/nservicebus/sqlserver/deployment-options.md) additional configuration is required for proper message routing:

 * The sending endpoint needs to know the connection string of the receiving endpoint.
 * The replying endpoint needs to know the connection string of the originator of the message for which the reply is being sent
 * The subscribing endpoint needs to know the connection string of the publishing endpoint, in order to send subscription request.
 * The publishing endpoint needs to know the connection strings or all the subscribed endpoints