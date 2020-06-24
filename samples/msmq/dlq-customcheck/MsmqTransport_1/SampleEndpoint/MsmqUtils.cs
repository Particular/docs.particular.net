using System;
using System.IO;
using System.Messaging;
using System.Security.Principal;
using System.Text;
using System.Transactions;

internal static class MsmqUtils
{
    static string fakeQueueName = "FakeQueue";

    public static void SendMessageToDeadLetterQueue(string messageBody)
    {
        using (var scope = new TransactionScope())
        {
            var path = QueuePath(fakeQueueName);
            using (var queue = new MessageQueue(path))
            using (var message = new Message())
            {
                message.UseDeadLetterQueue = true;
                message.TimeToBeReceived = TimeSpan.FromMilliseconds(50);
                var bytes = Encoding.UTF8.GetBytes(messageBody);
                message.BodyStream = new MemoryStream(bytes);
                queue.Send(message, MessageQueueTransactionType.Automatic);
            }
            scope.Complete();
        }
    }

    public static void SetUpDummyQueue()
    {
        CreateQueue(fakeQueueName, Environment.UserName);
    }

    static string QueuePath(string queueName) => $@"{Environment.MachineName}\private$\{queueName}";

    static void CreateQueue(string queueName, string account)
    {
        var path = QueuePath(queueName);
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