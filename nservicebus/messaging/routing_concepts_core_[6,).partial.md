NServiceBus routing consists of two layers, logical and physical. Logical routing defines which logical endpoint should receive a given outgoing message. Physical routing defines to which physical instance of the selected endpoint should the message be delivered. While logical routing is a developer's concern, physical routing is controlled by operations. 

[Broker transports](/transports/types.md#broker-transports) handle the physical routing automatically. Other transport might require specific configuration. See also [MSMQ Routing](/transports/msmq/routing.md).
