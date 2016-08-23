
### MessageDeserializationException (Versions 5.1 and above)

If a message fails to deserialize a `MessageDeserializationException` will be thrown by the `DeserializeLogicalMessagesBehavior`. In this case, the message is directly moved to the error queue to avoid blocking the system by poison messages.
