using System.Data;
using System.Data.Common;

namespace Hospitaly.Common.Application.Data;

public interface IDbConnectionFactory
{
   ValueTask<DbConnection> OpenConnectionAsync();
}