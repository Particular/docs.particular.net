---
title: A custom message viewer plugin for ServiceInsight
summary: A sample showing how a ServiceInsight plugin can be created and used to custom format the displayed messages.
component: ServiceInsight
related:
 - samples/encryption/message-body-encryption
reviewed: 2020-12-17
---

This sample shows how to a plugin for ServiceInsight can be created. The plugin model currently is designed to work as a custom message viewer, so a new tab will be displayed as part of the existing Body tab. This plugin is typically useful when:

- A custom serializer is used and existing Xml/Json viewers fail to deserialize and show it properly.
- Encryption on the message body (or certain properties) are used.

downloadbutton

## Running the project

Running the project will result in 2 console windows. Wait a moment until the ServicePulse window opens in the browser. In the console output of the Endpoint2, make a note of the ServiceControl connection URL. Finally, perform the following steps:

1. Copy over the output of the plugin project (ServiceInsight.Plugin) to the paths specified in the [documentation](/serviceinsight/custom-message-viewers.md#plugin-installation).
2. Run ServiceInsight and connect to the noted ServiceControl connection URL from above.
3. On the Messages window click on a message row.
4. Click on the Body tab and note that there is a new viewer tab called "Decryption Viewer".
5. Click on the Xml tab and notice how the message is in reverse (the demo 'encryption' function just reverses the body string).
6. Click on the new viewer tab and notice how the message is correctly displayed.

## Code walk-through 


### Plugin

The plugin is a project using 'WindowsDesktop' SDK. The views are WPF usercontrols and it is based on the same MVVM pattern ServiceInsight is using. There is a naming convention at play when it comes to names of views and the respective view model classes.

All the components require, including the views and view models are registered in the container by an Autofac module class.
	
snippet: IoCModule

The plugin must implement the `ICustomMessageBodyViewer` interface that gets invoked when a message is selected. The `Display` method has an argument that is the message object being displayed.

snippet: MessageDisplay

### Endpoint

The endpoint sends a message to a handler, and since the message has sensitive information, it encrypts the message before sending it, and then it is decrypted by the receiver. The related classes are in the Shared project so they are shared with the plugin.

The encryption and decryption objects are registered as message mutator at startup.
 
snippet: RegisterMessageEncryptor

snippet: MessageEncryptorExtension
