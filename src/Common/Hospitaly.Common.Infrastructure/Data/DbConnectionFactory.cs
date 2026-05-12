using System.Data.Common;
using Hospitaly.Common.Application.Data;
using Npgsql;

namespace Hospitaly.Common.Infrastructure.Data;

public class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}