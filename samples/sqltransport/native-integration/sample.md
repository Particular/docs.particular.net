---
title: SQL Server Native integration
summary: 'How to integrate natively with the SQL Server transport'
tags:
- SQL Server
- NHibernate
related:
- nservicebus/msmq
- nservicebus/sqlserver
- nservicebus/nhibernate
---

 1. Make sure you have SQL Server Express installed and accessible as `.\SQLEXPRESS`. Create a database `samples`
 2. Start the Receiver project.
 3. In the Sender's console you should see `Press any key to send a message. Press `q` to quit` text when the app is ready. 
 4. Hit any key.
 5. A message will be sent using ADO.NET and be received by the app.
 6. Open SQL Server Management Studio and go to the `samples` database.
 7. Open the Scripts.sql included in the sample.
 7. Run the `SendFromTSQL` statement.
 8. Notice how the app shows that a new message has been processed.
 9. Create the `Orders` table using the `CreateLegacyTable` statement.
 10. Create the insert trigger using the `CreateTrigger` statement.
 11. Right click the table you just created and do `Edit top X rows`.
 12. Notice that a message is received by the app for each "order" you create.
 

## Code walk-through

The first thing you need to do when doing native integration with the SQL Server transport is figure out where to insert your "message". The database and server names can easily be found by looking at the connection string and the table name is, by default, the same as your endpoint's name. So looking at our endpoint configuration

<!-- import EndpointConfiguration-->

The table would be `Samples.SqlServer.NativeIntegration` in the database `samples` on server `.\SQLEXPRESS` (localhost). Now that we know where to put the data, let's see how we can make sure that the endpoint can parse our message.

### Serialization

In this sample we'll be using JSON to serialize the messages but XML would have worked equally well. To configure the endpoint we just make call to `.UseSerialization<JsonSerializer>()` as shown above.

Now that our endpoint can understand JSON payloads we need define a message contract to use. In NServiceBus messages are plain C# classes so we just create the following class

<!-- import MessageContract-->

The final piece of the puzzle is to tell the serializer what C# class our JSON payload belongs to. We do this using the Json.NET `$type` attribute. The message body will then look as follows:

<!-- import MessagePayload-->

With this in place our endpoint can now parse the incoming JSON payload to a strongly typed message and invoke the correct message handlers.

### Sending the message

Now that we've done all the legwork, sending a message to our endpoint using plain ADO.NET is as easy as

<!-- import SendingUsingAdoNet-->

Armed with this you will now be able to send messages from any app in your organization that supports ADO.NET

## Sending from within the database

So far we've seen how to send from other .NET applications. While that is fine sometimes you'll integrate with old legacy apps where performing sends straight from within the database itself might be a better approach. Just execute the following T-SQL statement and notice how the message is consumed by your NServiceBus endpoint.

snippet:SendFromTSQL

### Using triggers to emit messages

Sometimes you're not allowed to touch the legacy systems you're dealing with and this is where triggers come in handy. Yes you read that right, triggers! while, rightfully so, considered evil by most sane people there are still use cases where a trigger might be just what we need. 

Let's create a fictive `Orders` table using

snippet:CreateLegacyTable

and create an `on inserted` trigger that will send a `LegacyOrderDetected` message for each new order that we add to the table. Here's the trigger:

snippet:CreateTrigger

Notice how we generate a unique message id by hashing the identity column. NServiceBus requires each message to have a unique id in order to safely perform retries.

That's it, just add a few orders to the table and notice how the app receives the messages. In a real life scenario you would likely use this to trigger a `bus.Publish` to push an `OrderAccepted` event out on the bus.
