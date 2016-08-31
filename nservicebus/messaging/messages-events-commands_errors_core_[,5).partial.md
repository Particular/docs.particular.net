 * "Pub/Sub is not supported for Commands. They should be sent direct to their logical owner." - this exception is being thrown when attempting to publish a Command or subscribe to/unsubscribe from a Command.
 * "Events can have multiple recipient so they should be published." - this exception will occur when attempting to use 'Send()' to send an event.
 * "Reply is neither supported for Commands nor Events. Commands should be sent to their logical owner using bus.Send and bus. Events should be Published with bus.Publish." - this exception is thrown when attempting to reply with a Command or an Event.
 * "Reply is not supported for Commands. Commands should be sent to their logical owner using bus.Send and bus." - this exception is thrown when one use reply with a Command.
 * "Reply is not supported for Events. Events should be Published with bus.Publish." - this exception will occur when one tries to use reply with an Event.
