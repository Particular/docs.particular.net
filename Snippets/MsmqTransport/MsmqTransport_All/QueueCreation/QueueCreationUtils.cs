namespace CoreAll.Msmq.QueueCreation
{
    using System;
    using System.Messaging;
    using System.Security.Principal;

    #region msmq-create-queues

    public static class QueueCreationUtils
    {

        public static void CreateQueue(string queueName, string account)
        {
            var path = $@"{Environment.MachineName}\private$\{queueName}";
            if (!MessageQueue.Exists(path))
            {
                using (var messageQueue = MessageQueue.Create(path, true))
                {
                    SetDefaultPermissionsForQueue(messageQueue, account);
                }
            }
        }

        static void SetDefaultPermissionsForQueue(MessageQueue queue, string account)
        {
            var allow = AccessControlEntryType.Allow;
            queue.SetPermissions(AdminGroup, MessageQueueAccessRights.FullControl, allow);

            queue.SetPermissions(account, MessageQueueAccessRights.WriteMessage, allow);
            queue.SetPermissions(account, MessageQueueAccessRights.ReceiveMessage, allow);
            queue.SetPermissions(account, MessageQueueAccessRights.PeekMessage, allow);
            queue.SetPermissions(account, MessageQueueAccessRights.GetQueueProperties, allow);
        }

        static string AdminGroup = GetGroupName(WellKnownSidType.BuiltinAdministratorsSid);

        static string GetGroupName(WellKnownSidType wellKnownSidType)
        {
            return new SecurityIdentifier(wellKnownSidType, null)
                .Translate(typeof(NTAccount))
                .ToString();
        }

    }

    #endregion

}