namespace Contracts;

public interface ISomethingHappened : IEvent
{
    int SomeData { get; set; }
}
