---
title: A custom message viewer plugin for ServiceInsight
summary: A sample showing how a ServiceInsight plugin can be created and used to custom format the displayed messages.
component: ServiceInsight
related:
 - samples/encryption/message-body-encryption
reviewed: 2020-12-17
---

This sample shows how to create a plugin for ServiceInsight. The plugin is designed to work as a custom message viewer, and provides a new tab inside the existing Body tab. The plugin is useful when:

- A custom serializer is used and existing XML/JSON viewers fail to deserialize and show it properly.
- The message body or some properties of the message are encrypted.

downloadbutton

## Running the project

Running the project will result in 2 console windows. Wait a moment until the ServicePulse window opens in the browser. In the console output of the Endpoint2, make a note of the ServiceControl connection URL. Finally, perform the following steps:

1. Copy over the output of the plugin project (ServiceInsight.Plugin) to the paths specified in the [documentation](/serviceinsight/custom-message-viewers.md#plugin-installation).
2. Run ServiceInsight and connect to the noted ServiceControl connection URL from above.
3. On the Messages window click on a message row.
4. Click on the Body tab and note that there is a new viewer tab called "Decryption Viewer".
5. Click on the XML tab and notice that the message body has been reversed (the demo "encryption" function just reverses the body string).
6. Click on the new tab and notice that the message is displayed correctly.

## Code walk-through

### Plugin

The plugin project uses the "WindowsDesktop" SDK. The views are WPF user controls and are based on the MVVM pattern used by ServiceInsight. There is a naming convention at play when it comes to names of views and the respective view model classes.

All the components, including the views and view models, must be registered in the container by an Autofac module class.

snippet: IoCModule

The plugin must implement the `ICustomMessageBodyViewer` interface that gets invoked when a message is selected. The `Display` method has an argument that is the message object being displayed.

snippet: MessageDisplay

### Endpoint

The endpoint sends a message to a handler. Since the message has sensitive information, it encrypts the message body and the body is decrypted by the receiver. The related classes are shared with the plugin via the Shared project.

The encryption and decryption objects are registered as message mutator at startup.

snippet: RegisterMessageEncryptor

snippet: MessageEncryptorExtension
