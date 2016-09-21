### Int

The integer response scenario allows any integer value to be returned in a strong typed manner.

#### Expose service

snippet:WcfIntCallback

#### Response

snippet:WcfIntCallbackResponse

### Enum

The enum response scenario allows any enum value to be returned in a strong typed manner.

#### Expose service

snippet:WcfEnumCallback

#### Response

snippet:WcfEnumCallbackResponse

### Object

The Object response scenario allows an object instance to be returned.


#### The Response message

This feature leverages the message Reply mechanism of the bus and hence the response need to be a message.

snippet:WcfCallbackResponseMessage


#### Expose service

snippet:WcfObjectCallback

partial:WcfFakeHandler

#### Response

snippet:WcfObjectCallbackResponse