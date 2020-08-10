namespace Core8.Logging
{
    using System;
    using NServiceBus;

    #region MessageWithToStringLogged

    public class MessageToLog :
        IMessage
    {
        public Guid EventId { get; set; }
        public DateTime? Time { get; set; }
        public TimeSpan Duration { get; set; }

        public override string ToString()
        {
            return $"MyMessage: EventId={EventId}, Time={Time}, Duration={Duration}";
        }
    }

    #endregion
}
