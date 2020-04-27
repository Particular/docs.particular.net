## Transport at a glance

|Feature                    |   |  
|:---                       |---
|Transactions |None, ReceiveOnly (Message visibility timeout)
|Pub/Sub                    |Native (Requires SNS, supports hybrid-mode for migration purposes)
|Timeouts                   |Native (Requires FIFO Queues)
|Large message bodies       |Native (Requires S3)
|Scale-out             |Competing consumer
|Scripted Deployment        |Built-in CLI, C#
|Installers                 |Optional