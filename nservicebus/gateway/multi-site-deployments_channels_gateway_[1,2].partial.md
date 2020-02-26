When the gateway is enabled it automatically sets up an HTTP channel to listen to `http://localhost/{name of the endpoint}`. To change this URL or add more than one incoming channel, configure `app.config`, as shown:

snippet: GatewayChannelsAppConfig

The `Default = true` on the first channel config entry tells the gateway which address to attach to an outgoing message if the sender does not specify it explicitly. Any number of channels can be added.

Or specify this physical routing in code using one of the techniques below.

#### Using an IConfigurationProvider

snippet: GatewayChannelsConfigurationProvider


#### Using a ConfigurationSource

snippet: GatewayChannelsConfigurationSource

Then at configuration time:

snippet: UseCustomConfigurationSourceForGatewayChannelsConfig