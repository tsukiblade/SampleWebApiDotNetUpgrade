using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
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

                services.AddDbContext<TodoContext>(opt => opt.UseSqlServer(_dbContainer.ConnectionString));
            });
        }


        public async Task Init()
        {
            await _dbContainer.StartAsync();

            var context = Services.GetRequiredService<TodoContext>();
            await context.Database.MigrateAsync();
        }

        public override ValueTask DisposeAsync()
        {
            _dbContainer.DisposeAsync();
            return base.DisposeAsync();
        }
    }
}
