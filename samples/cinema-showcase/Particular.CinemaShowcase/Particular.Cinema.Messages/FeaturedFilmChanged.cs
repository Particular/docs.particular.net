using NServiceBus;

namespace Particular.Cinema.Messages
{
    public class FeaturedFilmChanged : IEvent
    {
        public string? FeaturedFilmName { get; set; }
    }
}