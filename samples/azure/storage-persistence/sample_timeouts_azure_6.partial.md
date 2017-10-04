The timeout data from the `TimeoutDataTableName` table

```
  PartitionKey:= 2015090918
    RowKey:= 06800d44-9fc4-49b5-a9e9-a50e00ea76c0
    Destination:= Samples.Azure.StoragePersistence.Server@RETINA
    Headers:= {"NServiceBus.MessageId":"06800d44-9fc4-49b5-a9e9-...
    OwningTimeoutManager:= Samples.Azure.StoragePersistence.Server
    SagaId:= 21a6f7ed-65d2-42ff-a4d3-a50e00ea76ba
    StateAddress:= 06800d44-9fc4-49b5-a9e9-a50e00ea76c0
    Time:= 9/09/2015 6:06:59 PM
```

The timeout serialized message from the `timeoutstate` blob container.

```
'timeoutstate' container contents
  Blob:= 06800d44-9fc4-49b5-a9e9-a50e00ea76c0
    ﻿{"OrderDescription":"The saga for order 79cc2072-c724-4cc0-9202-b6c4918a3de2"}
```