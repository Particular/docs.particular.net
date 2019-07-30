namespace UsingClasses.Messages
{
#region immutable-messages-as-class
    public class MyMessage
    {
        public MyMessage(string data)
        {
            Data = data;
        }

        public string Data { get; private set; }
    }
#endregion
}