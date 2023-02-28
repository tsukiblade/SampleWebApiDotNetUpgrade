using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SampleWebApi.Models;

namespace SampleWebApi.IntegrationTests
{
    //In memory version of web api
    public class CustomWebAppFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        private readonly int _port = Random.Shared.Next(10000, 50000);
        private readonly MsSqlTestcontainer _dbContainer;

        public CustomWebAppFactory()
        {
            _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Password = "Pass@word",
                    Port = _port
                })
                .WithPortBinding(_port, 1433)
                .WithName($"MsSqlTestContainer{Guid.NewGuid()}")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TodoContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<TodoContext>(opt => opt.UseSqlServer(EnsureValidConnectionString(_dbContainer.ConnectionString, "SampleApiDb")));
            });
        }


        public async Task Init()
        {
            await _dbContainer.StartAsync();

            using var connection = new SqlConnection(EnsureValidConnectionString(_dbContainer.ConnectionString, null));
            await connection.OpenAsync();
            var command = new SqlCommand("CREATE DATABASE SampleApiDb", connection);
            await command.ExecuteNonQueryAsync();
            connection.Close();

            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
            await context.Database.MigrateAsync();
        }

        public override ValueTask DisposeAsync()
        {
            _dbContainer.DisposeAsync();
            return base.DisposeAsync();
        }

        private string EnsureValidConnectionString(string connectionString, string? databaseName)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            sqlConnectionStringBuilder.TrustServerCertificate = true;
            if (databaseName != null)
            {
                sqlConnectionStringBuilder.InitialCatalog = databaseName;
            }

            return sqlConnectionStringBuilder.ToString();
        }
    }
}
