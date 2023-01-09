using System;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Infraestructure.Data;
using IntegrationsTests.MockApis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestIntegrations;
using Xunit;

namespace IntegrationsTests
{
    public class TestApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        // Test container database instance
        private readonly TestcontainerDatabase _dbContainer;
        private MockChiliServer _chiliMockServer = new ();
        
        public TestApiFactory()
        {
            // Build the test container database using the TestcontainersBuilder class
            _dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    // Set the password for the test container database
                    Password = "localdevpassword#123",
                    // Set the name of the test container database
                    Database = "integration-db"
                })
                // Set the image for the test container database
                .WithImage("mcr.microsoft.com/mssql/server:2017-latest")
                // Set the clean up flag for the test container database
                .WithCleanUp(true)
                .Build();
            
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Customize the services available to the application during the test
            builder.ConfigureTestServices(services =>
            {
                // Remove the app's ApplicationDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<AppDbContext>));
            
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                // Add the AppDbContext to the service collection with a connection string
                // that points to the test container database
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        _dbContainer.ConnectionString));
                
                //override the HttpClientFactory
                services.AddHttpClient("myClient", config =>
                {
                    config.BaseAddress = new Uri(_chiliMockServer.Url);
                    config.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                // Get an instance of the AppDbContext from the service provider
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<AppDbContext>();
                // Apply any outstanding database migrations
                context.Database.Migrate();
                // Seed the database with test data
                Seed.SeedUsers(context); 
                
            });
        }
        
        // Start the test container database before the integration tests are run
        public async Task InitializeAsync()
        {
            //Start MockServers
            _chiliMockServer.StartServer();
            //SetUp the mapping request
            _chiliMockServer.SetUpGetChiliApiKey();
            
            await _dbContainer.StartAsync();
        }

        // Stop the test container database after the integration tests are run
        public  async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
            _chiliMockServer.Dispose();
        }
    }
}