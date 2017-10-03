
### DoNotCreateQueuesOnInstall

Starting from Versions 1.0 and above, the NServiceBus.Transport.Msmq NuGet package includes two PowerShell scripts to help facilitate the queue creation for the endpoints. In NServiceBus Versions 6 and below, the queues for the endpoints can be created via code by calling the [EnableInstaller configuration Api](/nservicebus/operations/installers.md), however the disadvantage is that the code to create the queues is being run every time during endpoint startup.

Call the `DoNotCreateQueuesOnInstall` configuration Api to explicitly not create the queues even when the EnableInstaller is called.
 
snippet: do-not-create-queues-on-install

