using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework
{
    #region SubmittedOrderEF

    [Table("SubmittedOrder", Schema = "receiver")]
    public class SubmittedOrder
    {
        public string Id { get; set; }
        public int Value { get; set; }
    }

    #endregion
}