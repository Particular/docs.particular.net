using NServiceBus;

namespace Core_9;

#region Command

public class DoSomething :
    ICommand
{
    public string SomeProperty { get; set; }
}

#endregion