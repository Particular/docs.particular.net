using System;
using System.Messaging;
using System.Security.Principal;

namespace Operations.Msmq
{


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
                using (var messageQueue = new MessageQueue(path))
                {
                    SetPermissionsForQueue(messageQueue, account);
                    return;
                }
            }
            using (var messageQueue = MessageQueue.Create(path, true))
            {
                SetPermissionsForQueue(messageQueue, account);
            }
        }

        static void SetPermissionsForQueue(MessageQueue queue, string account)
        {
            queue.SetPermissions(AdminGroup, MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
            queue.SetPermissions(EveryoneGroup, MessageQueueAccessRights.WriteMessage, AccessControlEntryType.Allow);
            queue.SetPermissions(AnonymousLogon, MessageQueueAccessRights.WriteMessage, AccessControlEntryType.Allow);

            queue.SetPermissions(account, MessageQueueAccessRights.WriteMessage, AccessControlEntryType.Allow);
            queue.SetPermissions(account, MessageQueueAccessRights.ReceiveMessage, AccessControlEntryType.Allow);
            queue.SetPermissions(account, MessageQueueAccessRights.PeekMessage, AccessControlEntryType.Allow);
        }

        static string AdminGroup = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null)
            .Translate(typeof(NTAccount))
            .ToString();

        static string EveryoneGroup = new SecurityIdentifier(WellKnownSidType.WorldSid, null)
            .Translate(typeof(NTAccount))
            .ToString();

        static string AnonymousLogon = new SecurityIdentifier(WellKnownSidType.AnonymousSid, null)
            .Translate(typeof(NTAccount))
            .ToString();

        #endregion
    }

}