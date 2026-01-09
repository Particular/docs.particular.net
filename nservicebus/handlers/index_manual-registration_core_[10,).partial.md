## Manual handler registration

By default, message handlers are automatically discovered through [assembly scanning](/nservicebus/hosting/assembly-scanning.md). When [assembly scanning is disabled](/nservicebus/hosting/assembly-scanning-disable.md), handlers must be manually registered using `AddHandler<THandler>()`:

snippet: RegisterHandlerManually
