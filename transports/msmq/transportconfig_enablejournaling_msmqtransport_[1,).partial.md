
### EnableJournaling

This API enables the use of journaling messages. With this option, MSMQ will store a copy of every message received and processed in the [journal queue](https://msdn.microsoft.com/en-us/library/ms702011.aspx). 
 
snippet: enable-journaling

WARNING: This setting can potentially use up the MSMQ storage quota based on the message volume.

