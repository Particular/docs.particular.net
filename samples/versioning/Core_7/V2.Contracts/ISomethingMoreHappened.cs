namespace Contracts
{
    #region V2Message

    public interface ISomethingMoreHappened : ISomethingHappened
    {
        string MoreInfo { get; set; }
    }

    #endregion
}
