While these URLs can be placed directly in the call, it is recommended to put these settings in `app.config` so administrators can change them should the need arise. To do this, add this config section:

snippet: GatewaySitesAppConfig

Or specify this physical routing in code using one of the techniques below.

#### Using an IConfigurationProvider

snippet: GatewaySitesConfigurationProvider

#### Using a ConfigurationSource

snippet: GatewaySitesConfigurationSource

Then at configuration time:

snippet: UseCustomConfigurationSourceForGatewaySitesConfig