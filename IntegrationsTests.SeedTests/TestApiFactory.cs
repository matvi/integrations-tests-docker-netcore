using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infraestructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TestIntegrations;
using TestIntegrations.Data;
using Xunit;

namespace IntegrationsTests.SeedTests
{
    public class TestApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        private readonly TestcontainerDatabase _dbContainer;
        
        public TestApiFactory()
        {
            _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Password = "localdevpassword#123",
                    Database = "integration-db"
                })
                .WithImage("mcr.microsoft.com/mssql/server:2017-latest")
                .WithCleanUp(true)
                .Build();
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
        
            builder.ConfigureTestServices(services =>
            {
            
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<AppDbContext>));
            
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                //services.RemoveAll(typeof(DbContext));
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        _dbContainer.ConnectionString));
                
                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
                Seed.SeedUsers(context); 
                
            });
            
            
        }
        
        
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        public  async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}