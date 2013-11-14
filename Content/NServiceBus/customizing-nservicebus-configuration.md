<!--
title: "Customizing NServiceBus Configuration"
tags: ""
summary: "<p>NServiceBus uses the process config file as its default source of configuration. The pluggability and extensibility of NServiceBus allow you to change many of its behaviors, including where it gets its configuration. This can be done across all of NServiceBus or you can choose which part of NServiceBus should get its configuration from some other source.</p>
<h2>Overriding App.Config section</h2>
"
-->

NServiceBus uses the process config file as its default source of configuration. The pluggability and extensibility of NServiceBus allow you to change many of its behaviors, including where it gets its configuration. This can be done across all of NServiceBus or you can choose which part of NServiceBus should get its configuration from some other source.

Overriding App.Config section
-----------------------------

The preferred method of overriding a specific section is to use the
[IProvideConfiguration<t>](https://github.com/NServiceBus/NServiceBus/blob/master/src/NServiceBus.Core/Config/ConfigurationSource/IConfigurationSource.cs#L23) model. For example, rather than providing the RijndaelEncryptionServiceConfig in app.config, you could provide it in code:

<script src="https://gist.github.com/Particular/6107912.js?file=ProvideConfig.cs"></script> Code only configuration
-----------------------

If you don't want your process to have its configuration specified in the config file, you can write a class that implements IConfigurationSource and in it retrieve the configuration from any location you like: a database, a web service, anything. Here's how:

<script src="https://gist.github.com/Particular/6107912.js?file=CustomConfigSource.cs"></script> The initialization code instructs NServiceBus to use a CustomConfigurationSource, passing in an instance of a new object: MyCustomConfigurationSource. Its GetConfiguration method provides data for RijndaelEncryptionServiceConfig directly in code, while allowing all other configuration sections to be retrieved from the config file.

**IMPORTANT** : Add a reference to System.Configuration to use the ConfigurationManager object.

To do this when using the NServiceBus host, implement IWantCustomInitialization but this time on the class [implementing IConfigureThisEndpoint](the-nservicebus-host.md) .

