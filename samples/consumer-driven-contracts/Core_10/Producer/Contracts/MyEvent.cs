namespace Publisher.Contracts;

using Subscriber1.Contracts;
using Subscriber2.Contracts;

#region publisher-contracts
class MyEvent :
    Consumer1Contract,
    Consumer2Contract
{
    public string Consumer1Property { get; set; }
    public string Consumer2Property { get; set; }
}
#endregion