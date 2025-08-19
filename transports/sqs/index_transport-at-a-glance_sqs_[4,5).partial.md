## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions |None, ReceiveOnly (Message visibility timeout)
|Pub/Sub                    |Message driven
|Timeouts                   |Native (Requires FIFO Queues)
|Large message bodies       |Native (Requires S3)
|Scale-out             |Competing consumer
|Scripted Deployment        |PowerShell, CloudFormation, C#
|Installers                 |Optional
|Native integration         |Not supported
|Case Sensitive             |Yes
|Local development          |[Supported via LocalStack](/nservicebus/aws/local-development.md)
