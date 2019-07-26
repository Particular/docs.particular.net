namespace UsingClasses.Messages
{
    public class MyMessage
    {
        public MyMessage(string data)
        {
            Data = data;
        }

        public string Data { get; private set; }
    }
}