using System.Data;

namespace Movies.Application.Database;

public interface IDBConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
