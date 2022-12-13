using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationsTests.SeedTests
{
    public class UnitTest1 : IClassFixture<TestApiFactory>
    {
        private readonly TestApiFactory _testApiFactory;

        public UnitTest1(TestApiFactory testApiFactory)
        {
            _testApiFactory = testApiFactory;
        }
        [Fact]
        public async Task Test1()
        {
            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User");

            var data = await result.Content.ReadAsStringAsync();
            
            //await Task.Delay(150000);
        }
    }
}