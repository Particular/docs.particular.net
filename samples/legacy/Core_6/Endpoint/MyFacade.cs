class MyFacade
{
    MyService myService;
    MyOtherService otherService;

    public MyFacade(MyService myService, MyOtherService otherService)
    {
        this.myService = myService;
        this.otherService = otherService;
    }

    public void Do(decimal value)
    {
        myService.DoOneThing(value);
        otherService.DoOtherThing();
    }
}