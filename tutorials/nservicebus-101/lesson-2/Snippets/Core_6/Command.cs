using NServiceBus;

namespace Core_6
{
    #region Command

    public class DoSomething : ICommand
    {
        public string SomeProperty { get; set; }
    }

    #endregion
}