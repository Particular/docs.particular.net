using System;

namespace Messages.Events
{
    public class MassTransitEvent : IMTEvent
    {
        public string Text { get; set; }
    }

    public interface IMTEvent { }
}
