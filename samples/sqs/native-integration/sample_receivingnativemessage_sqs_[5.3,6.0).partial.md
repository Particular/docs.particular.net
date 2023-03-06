On the receiving end, an NServiceBus endpoint is listening to the queue and has a handler in place to handle messages of type `SomeNativeMessage`.

First, the message will be intercepted in the incoming logical message context as there is a behavior in place:

snippet: BehaviorAccessingNativeMessage

The code to register the above behavior is:

snippet: RegisterBehaviorInPipeline

Next, the handler is invoked. The handler code can also access the native message and its attributes.

Note: The message attribute `MessageTypeFullName` might not be available anymore in the `MessageAttributes` collection in recoverability scenarios. Instead, it will be part of the `Headers` collection.

snippet: HandlerAccessingNativeMessage


