class MyOtherService
{
    IBus bus;

    public MyOtherService(IBus bus)
    {
        this.bus = bus;
    }

    public void DoOtherThing()
    {
        bus.SendLocal(new DownstreamMessage());
    }
}