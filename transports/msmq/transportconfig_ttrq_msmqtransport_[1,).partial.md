
### TimeToReachQueue

Overrides the TTRQ timespan. The default value if not set is Message.InfiniteTimeout.

TTRQ is the time limit for the message to reach the destination queue, beginning from the time the message is sent. This sets the underlying [Message.TimeToReachQueue](https://msdn.microsoft.com/en-us/library/system.messaging.message.timetoreachqueue). 

Format must be compatible with [TimeSpan.Parse](https://msdn.microsoft.com/en-us/library/se73z7b9). 
 
snippet: time-to-reach-queue

