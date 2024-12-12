using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace TechChallenge.Infra
{
    public class DbConnectionProvider: IDisposable
    {
        private IDbConnection _connection;
        private IConfiguration _configuration { get; set; }
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
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
