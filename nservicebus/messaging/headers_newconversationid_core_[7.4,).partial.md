#### Starting a new conversation

In some scenarios though, starting a new conversation might be desirable. For example, a batch import that reads thousands of records and starts a workflow on each one would normally result in a giant visualization, where it would be more appropriate for each record to be a new conversation.

Starting a new conversation can be done with the help of SendOptions:

snippet: new-conversation-id

A custom `Conversation ID` can also be provided:

snippet: new-conversation-custom-id
