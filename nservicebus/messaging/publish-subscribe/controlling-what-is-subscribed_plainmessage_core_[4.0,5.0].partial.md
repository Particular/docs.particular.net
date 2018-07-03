In NServiceBus versions 4 and 5, it is possible to opt-in to automatically subscribe to all messages not defined as a command using `ICommand` or the `.DefiningCommandsAs` convention using the following code:

snippet: AutoSubscribePlainMessages