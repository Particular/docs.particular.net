### Int

The integer response scenario allows any integer value to be returned in a strong typed manner.

NOTE: The receiving endpoint requires a reference to `NServiceBus.Callbacks`.


#### Expose service

snippet:WcfIntCallback


#### Response

snippet:WcfIntCallbackResponse


### Enum

The enum response scenario allows any enum value to be returned in a strong typed manner.

NOTE: The receiving endpoint requires a reference to `NServiceBus.Callbacks`.


#### Expose service

snippet:WcfEnumCallback


#### Response

snippet:WcfEnumCallbackResponse


### Object

The Object response scenario allows an object instance to be returned.

NOTE: The receiving endpoint does not require a reference to `NServiceBus.Callbacks`.


#### The Response message

This feature leverages the message Reply mechanism of the bus and hence the response need to be a message.

snippet:WcfCallbackResponseMessage


#### Expose service

snippet:WcfObjectCallback


#### Response

snippet:WcfObjectCallbackResponse