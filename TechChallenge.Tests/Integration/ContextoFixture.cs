using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Infra.Context;
using Testcontainers.PostgreSql;

namespace TechChallenge.Tests.Integration
{
    public class ContextoFixture : IAsyncLifetime
    {
        public MainContext Context { get; private set; }
        private readonly PostgreSqlContainer _npgsqlContainer = new PostgreSqlBuilder().WithImage("postgres:latest").Build();

        public async Task InitializeAsync()
        {
            await _npgsqlContainer.StartAsync();
            var options = new DbContextOptionsBuilder<MainContext>()
               .UseNpgsql(_npgsqlContainer.GetConnectionString())
               .Options;

            Context = new MainContext(options);
            Context.Database.Migrate();
        }

        public async Task DisposeAsync()
        {
            await _npgsqlContainer.StopAsync();
        }
    }
}
