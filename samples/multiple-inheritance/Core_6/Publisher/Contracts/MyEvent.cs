namespace Publisher.Contracts
{
    using Subscriber1.Contracts;
    using Subscriber2.Contracts;

    class MyEvent : Subscriber1Event, Subscriber2Event
    {
        public string Subscriber1Property { get; set; }
        public string Subscriber2Property { get; set; }
    }
}