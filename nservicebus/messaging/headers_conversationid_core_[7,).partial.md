To get full control over the `ConversationId`, a custom convention can be registered:

snippet: custom-conversation-id-convention

This will automatically be invoked for all sent messages so they use the custom `ConversationId`, and it no longer needs to be manually set for each individual message. This approach is useful when a given rule or custom attribute should be applied to all sent messages.

> [!NOTE]
> This is not invoked for incoming messages.
