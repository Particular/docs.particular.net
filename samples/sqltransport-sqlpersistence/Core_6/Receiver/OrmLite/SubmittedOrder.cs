namespace OrmLite
{
    using ServiceStack.DataAnnotations;

    [Schema("receiver")]
    public class SubmittedOrder
    {
        public string Id { get; set; }
        public int Value { get; set; }
    }
}