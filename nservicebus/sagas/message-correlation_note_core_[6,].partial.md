{{NOTE:
NServiceBus will enforce that all correlated properties have a non default value when the saga instance is persisted. This means that all messages starting a saga must have a mapping.

NServiceBus does not support changing the value of correlated properties for existing instances.
}}
