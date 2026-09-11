## Migration analyzer

The NServiceBus package also includes a migration analyzer for the messaging APIs. It reports the NSB0039, NSB0040, and NSB0041 diagnostics when messages are sent, published, replied to, or updated using the object-based overloads that are not trimming-safe, and when logical message mutators replace a message using its runtime type. See [Trimming-safe messaging overloads](/nservicebus/messaging/trimming-safe-messaging-overloads.md) for details.
