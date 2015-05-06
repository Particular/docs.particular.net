using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
