namespace CoreAll.Msmq.QueueCreation
{
    using System;

    class QueueCreationUtilsUsage
    {

        QueueCreationUtilsUsage()
        {

            #region msmq-create-queues-shared-usage

            QueueCreationUtils.CreateQueue(
                queueName: "error",
                account: Environment.UserName);

            QueueCreationUtils.CreateQueue(
                queueName: "audit",
                account: Environment.UserName);

            #endregion
        }

    }

}