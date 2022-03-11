namespace Testing_8.Saga
{
    using NServiceBus;

    public class StartsSaga :
        ICommand
    {
        public string MyId { get; set; }
    }
}