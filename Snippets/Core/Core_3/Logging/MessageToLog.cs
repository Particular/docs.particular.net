namespace Core3.Logging
{
    using System;
    using NServiceBus;

    #region MessageWithToStringLogged

    public class MessageToLog : IMessage
    {
        public Guid EventId { get; set; }
        public DateTime? Time { get; set; }
        public TimeSpan Duration { get; set; }

        public override string ToString()
        {
            return string.Format(
                "MyMessage: EventId={0}, Time={1}, Duration={2}",
                EventId, Time, Duration
                );
        }
    }

    #endregion
}
