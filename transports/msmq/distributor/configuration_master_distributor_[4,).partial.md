It is also possible to use the `NServiceBus.MSMQMaster` profile to start distributor and worker processes on the endpoint. To start the endpoint as a Master install the [NServiceBus.Distributor.MSMQ NuGet](https://www.nuget.org/packages/NServiceBus.Distributor.MSMQ) and then run the host from the command line, as follows:

```dos
NServiceBus.Host.exe NServiceBus.MSMQMaster
```
