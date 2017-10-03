
### DisableInstaller

The NuGet package includes two PowerShell scripts to help facilitate the queue creation and cleanup for an endpoint. The queues for the endpoint can be created via code by calling the [EnableInstaller configuration Api](/nservicebus/operations/installers.md), the code to create the queues is run every time the endpoint starts up.

To avoid this behavior, call this transport configuration Api to explicitly not create the queues even when the endpoint explicitly invokes the `EnableInstaller`.
 
snippet: disable-installer

