using System.Data.Entity.Core.EntityClient;

namespace SqlServer_All.EfEdmx
{

    #region DbContextPartialWithEntityConnection

    partial class MySampleContainer
    {
        public MySampleContainer(EntityConnection dbConnection)
            : base(dbConnection, true)
        {
        }
    }

    #endregion
}