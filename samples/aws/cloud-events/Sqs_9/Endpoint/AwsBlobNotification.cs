#region sqs-cloudevents-message-definition
public class AwsBlobNotification :
    IMessage
{
    public string Key { get; set; }
    public int Size { get; set; }
    public string ETag { get; set; }
    public string Sequencer { get; set; }
}
#endregion
