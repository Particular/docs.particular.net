NServiceBus can provide transactional consistency between incoming and outgoing messages:

```csharp
[assembly: NServiceBusTriggerFunction("MyEndpoint", SendsAtomicWithReceive = true)]
```

This is the equivalent to the [sends atomic with receive](/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive) transport transaction mode.

### Without trigger function attribute

If the Azure Function queue trigger attribute is not being used, then NServiceBus will process messages transactionally if it can control the receive transaction. This is done by looking for `ServiceBusTriggerAttribute` in the call stack and checking the `AutoComplete` property.

If auto-complete is enabled, which is the default, then NServiceBus cannot control the receive transaction and the message is processed non-transactionally.

```csharp
[FunctionName("ProcessMessage")]
public async Task Run(
    // Setting AutoComplete to true (the default) processes the message non-transactionally
    [ServiceBusTrigger("ProcessMessage", AutoComplete = true)]
    Message message,
    ILogger logger,
    IMessageReceiver messageReceiver,
    ExecutionContext executionContext)
{
    await endpoint.Process(message, executionContext, messageReceiver, logger);
}
```

If auto-complete is not enabled then NServiceBus can control the receive transaction and the message is processed transactionally.

```csharp
[FunctionName("ProcessMessageTx")]
public async Task Run(
    // Setting AutoComplete to false processes the message transactionally
    [ServiceBusTrigger("ProcessMessageTx", AutoComplete = false)]
    Message message,
    ILogger logger,
    IMessageReceiver messageReceiver,
    ExecutionContext executionContext)
{
    await endpoint.Process(message, executionContext, messageReceiver, logger);
}
```

If additional control is required, or the service bus triger is not configured using an attribute, use the concrete function endpoint class.

```csharp
class MyFunctions
{
    const bool EnableTransactions = true;

    // NOTE: Use concrete class instead of interface
    readonly FunctionEndpoint endpoint;

    public MyFunctions(FunctionEndpoint endpoint)
    {
        this.endpoint = endpoint;
    }

    [FunctionName("ProcessMessages")]
    public async Task Run(
        [ServiceBusTrigger("ProcessMessages", AutoComplete = !EnableTransactions)]
        Message message,
        ILogger logger,
        IMessageReceiver messageReceiver,
        ExecutionContext executionContext)
    {
        if(EnableTransactions)
        {
            await endpoint.ProcessTransactional(message, executionContext, messageReceiver, logger);
        }
        else
        {
            await endpoint.ProcessNonTransactional(message, executionContext, messageReceiver, logger)
        }
    }
}
```

DANGER: Incorrectly configuring the service bus trigger auto-complete setting can lead to message loss. It is recommended to use the auto-detection mechanism on the function endpoint interface, or to use the trigger function attribute to specify message consistency.