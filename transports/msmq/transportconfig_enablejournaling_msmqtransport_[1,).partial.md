
### EnableJournaling

This API Enables the use of journaling messages. With this option, MSMQ will store a copy of every message received and processed in the [journal queue](https://msdn.microsoft.com/en-us/library/ms702011.aspx). 
 
snippet: enable-journaling

WARN: This setting should be used ONLY when debugging as it can potentially use up the MSMQ storage quota based on the message volume.

