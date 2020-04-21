
### DisableInstaller

The NuGet package includes two PowerShell scripts to help facilitate the queue creation and cleanup for an endpoint. The queues for the endpoint can be created via code by calling the [EnableInstaller configuration API](/nservicebus/operations/installers.md). The code to create the queues is run every time the endpoint starts up.

To avoid queues being created when installers are enabled on the endpoint (using `endpointConfiguration.EnableInstallers()`), call this transport configuration API to explicitly _not_ create the queues:
 
snippet: disable-installer

