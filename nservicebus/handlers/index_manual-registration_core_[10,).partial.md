## Manual handler registration

By default, message handlers are automatically discovered through [assembly scanning](/nservicebus/hosting/assembly-scanning.md?version=core_10). When [assembly scanning is disabled](/nservicebus/hosting/assembly-scanning.md?version=core_10#disable-assembly-scanning), handlers must be manually registered using `AddHandler<THandler>()`:

snippet: RegisterHandlerManually
