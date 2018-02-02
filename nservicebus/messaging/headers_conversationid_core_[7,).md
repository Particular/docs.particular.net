To get full control over the Conversation Id a custom convention can be registered:

snippet: custom-conversation-id-convention

This is useful to avoid setting the conversation id when sending individual messages but rather apply some kind of convention using a custom attribute, inherit from some interface, use reflection based on message types, etc.
