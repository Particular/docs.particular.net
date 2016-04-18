namespace Operations.Msmq
{
    using System;
    using System.Messaging;
    using System.Security.Principal;

    public static class QueueCreation
    {

        public static void Usage()
        {
            #region msmq-create-queues-endpoint-usage

            CreateQueuesForEndpoint(
                endpointName: "myendpoint",
                account: Environment.UserName);

            #endregion

            #region msmq-create-queues-shared-usage

            CreateQueue(
                queueName: "error",
                account: Environment.UserName);

            CreateQueue(
                queueName: "audit",
                account: Environment.UserName);

            #endregion
        }

        #region msmq-create-queues

        public static void CreateQueuesForEndpoint(string endpointName, string account)
        {
            //main queue
            CreateQueue(endpointName, account);

            //retries queue
            CreateQueue(endpointName + ".retries", account);

            //timeout queue
            CreateQueue(endpointName + ".timeouts", account);

            //timeout dispatcher queue
            CreateQueue(endpointName + ".timeoutsdispatcher", account);
        }

        public static void CreateQueue(string queueName, string account)
        {
            string path = string.Format(@"{0}\private$\{1}", Environment.MachineName, queueName);
            if (MessageQueue.Exists(path))
            {
                using (MessageQueue messageQueue = new MessageQueue(path))
                {
                    SetPermissionsForQueue(messageQueue, account);
                    return;
                }
            }
            using (MessageQueue messageQueue = MessageQueue.Create(path, true))
            {
                SetPermissionsForQueue(messageQueue, account);
            }
        }

        static void SetPermissionsForQueue(MessageQueue queue, string account)
        {
            AccessControlEntryType allow = AccessControlEntryType.Allow;
            queue.SetPermissions(AdminGroup, MessageQueueAccessRights.FullControl, allow);
            queue.SetPermissions(EveryoneGroup, MessageQueueAccessRights.WriteMessage, allow);
            queue.SetPermissions(AnonymousLogon, MessageQueueAccessRights.WriteMessage, allow);

            queue.SetPermissions(account, MessageQueueAccessRights.WriteMessage, allow);
            queue.SetPermissions(account, MessageQueueAccessRights.ReceiveMessage, allow);
            queue.SetPermissions(account, MessageQueueAccessRights.PeekMessage, allow);
        }

        static string AdminGroup = GetGroupName(WellKnownSidType.BuiltinAdministratorsSid);
        static string EveryoneGroup = GetGroupName(WellKnownSidType.WorldSid);
        static string AnonymousLogon = GetGroupName(WellKnownSidType.AnonymousSid);

        static string GetGroupName(WellKnownSidType wellKnownSidType)
        {
            return new SecurityIdentifier(wellKnownSidType, null)
                .Translate(typeof(NTAccount))
                .ToString();
        }

        #endregion
    }

}