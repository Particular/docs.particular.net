using UsingInterfaces.Messages;

namespace Messages
{
#region immutable-messages-as-interface-implementation
    class MyMessageImpl : IMyMessage
    {
        public string Data { get; set; }
    }
#endregion
}