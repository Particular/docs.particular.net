using NServiceBus;

#region asb-function-isolated-identity-connection
[assembly: NServiceBusTriggerFunction("WorkerDemoEndpoint", Connection = "MyConnectionName")]
#endregion
public class Program_with_connection
{
}