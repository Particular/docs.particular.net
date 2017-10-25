
### EnableJournaling

This API Enables the use of journaling messages. With this option, MSMQ will store a copy of every message received in the journal queue that has been processed. 
 
snippet: enable-journaling

WARN: This setting should be used ONLY when debugging as it can potentially use up the MSMQ journal storage quota based on the message volume.

