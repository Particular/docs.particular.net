
### MSMQ Label

WARNING: This feature was added in Version 6 and can be used to communicate with Version 5 (and higher) endpoints. However it should **not** be used when communicating to earlier versions (2, 3 or 4) since in those versions the MSMQ Label was used to communicate certain NServiceBus implementation details.

Often when debugging MSMQ using [native tools](viewing-message-content-in-msmq.md) it is helpful to have some custom text in the MSMQ Label. For example the message type or the message id. As of Version 6 the text used to apply to [Message.Label](https://msdn.microsoft.com/library/system.messaging.message.label.aspx) can be controlled at configuration time using the `ApplyLabelToMessages` extension method. This method takes a delegate which will be passed the header collection and should return a string to use for the label. It will be called for all standard messages as well as Audits, Errors and all control messages. The only exception to this rule is received messages with corrupted headers. In some cases it may be useful to use the `Headers.ControlMessageHeader` key to determine if a message is a control message. These messages will be forwarded to the error queue with no label applied. The returned string can be `String.Empty` for no label and must be at most 240 characters.

snippet:ApplyLabelToMessages

