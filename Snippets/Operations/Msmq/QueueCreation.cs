using System;
using System.Messaging;
using System.Security.Principal;

namespace Operations.Msmq
{

    //using System.Messaging.dll
    public static class QueueCreation
    {
        public static void DeleteAllQueues()
        {
            var machineQueues = MessageQueue.GetPrivateQueuesByMachine(".");
            foreach (var q in machineQueues)
            {
                MessageQueue.Delete(q.Path);
            }
        }

        public static void DeleteQueue(string queueName)
        {
            string path = Environment.MachineName + "\\private$\\" + queueName;
            if (MessageQueue.Exists(path))
            {
                MessageQueue.Delete(path);
            }
        }

        public static void DeleteQueuesForEndpoint(string endpointName)
        {
            //main queue
            DeleteQueue(endpointName);

            //retries queue
            DeleteQueue(endpointName + ".retries");

            //timeout queue
            DeleteQueue(endpointName + ".timeouts");

            //timeout dispatcher queue
            DeleteQueue(endpointName + ".timeoutsdispatcher");
        }

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
            string msmqQueueName = Environment.MachineName + "\\private$\\" + queueName;
            if (MessageQueue.Exists(msmqQueueName))
            {
                using (var messageQueue = new MessageQueue(msmqQueueName))
                {
                    SetPermissionsForQueue(messageQueue, account);
                    return;
                }
            }
            using (var messageQueue = MessageQueue.Create(msmqQueueName, true))
            {
                SetPermissionsForQueue(messageQueue, account);
            }
        }

        static void SetPermissionsForQueue(MessageQueue queue, string account)
        {
            queue.SetPermissions(LocalAdministratorsGroupName, MessageQueueAccessRights.FullControl, AccessControlEntryType.Allow);
            queue.SetPermissions(LocalEveryoneGroupName, MessageQueueAccessRights.WriteMessage, AccessControlEntryType.Allow);
            queue.SetPermissions(LocalAnonymousLogonName, MessageQueueAccessRights.WriteMessage, AccessControlEntryType.Allow);

            queue.SetPermissions(account, MessageQueueAccessRights.WriteMessage, AccessControlEntryType.Allow);
            queue.SetPermissions(account, MessageQueueAccessRights.ReceiveMessage, AccessControlEntryType.Allow);
            queue.SetPermissions(account, MessageQueueAccessRights.PeekMessage, AccessControlEntryType.Allow);
        }

        static string LocalAdministratorsGroupName = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null).Translate(typeof(NTAccount)).ToString();
        static string LocalEveryoneGroupName = new SecurityIdentifier(WellKnownSidType.WorldSid, null).Translate(typeof(NTAccount)).ToString();
        static string LocalAnonymousLogonName = new SecurityIdentifier(WellKnownSidType.AnonymousSid, null).Translate(typeof(NTAccount)).ToString();
    }

}