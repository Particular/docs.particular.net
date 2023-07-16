using NServiceBus;

namespace Core_8
{
    #region Command

    public class DoSomething :
        ICommand
    {
        public string SomeProperty { get; set; }
    }

    #endregion
}