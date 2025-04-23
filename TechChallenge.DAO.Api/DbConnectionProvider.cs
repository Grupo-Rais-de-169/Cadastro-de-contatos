using Npgsql;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace TechChallenge.DAO.Api
{
    [ExcludeFromCodeCoverage]
    public class DbConnectionProvider: IDisposable
    {
        private IDbConnection _connection;
        private readonly IConfiguration _configuration;
        private bool _disposed = false;
        public DbConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSQL"));
                _connection.Open();
            }
            return _connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
            _disposed = true;
        }
        ~DbConnectionProvider()
        {
            Dispose(false);
        }
    }
}
