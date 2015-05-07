using System.Data.Entity.Core.EntityClient;

namespace Snippets_Misc.EF_EDMX
{
	#region DbContextPartialWithEntityConnection
	partial class MySampleContainer
	{
		public MySampleContainer( EntityConnection dbConnection )
			: base( dbConnection, true ) { }
	}
	#endregion
}
