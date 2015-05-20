using System;
using System.Messaging;
using System.Security.Principal;

namespace Operations.Msmq
{
    //using System.Messaging.dll
    public static class QueueCreation
    {
        public static void CreateAllForEndpoint(string endpointName, string account)
        {
            //main queue
            CreateLocalQueue(endpointName, account);

            //retries queue
            CreateLocalQueue(endpointName + ".retries", account);

            //timeout queue
            CreateLocalQueue(endpointName + ".timeouts", account);

            //timeout dispatcher queue
            CreateLocalQueue(endpointName + ".timeoutsdispatcher", account);
        }

        public static void CreateLocalQueue(string queueName, string account)
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