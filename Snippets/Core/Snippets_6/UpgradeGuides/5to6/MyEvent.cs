namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus;

    public class MyEvent : IEvent
    {
        public string Data { get; set; }
    }
}