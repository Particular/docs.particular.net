using System.Data.Entity.Core.EntityClient;

namespace Snippets_Misc.EfEdmx
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