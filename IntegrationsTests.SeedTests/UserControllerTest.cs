using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Entities;
using FluentAssertions;
using Org.BouncyCastle.Asn1;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationsTests.SeedTests
{
    [TestCaseOrderer(typeof(PriorityOrderer)]
    public class UserControllerTest : IClassFixture<TestApiFactory>
    {
        private readonly TestApiFactory _testApiFactory;
        private readonly ITestOutputHelper _testOutputHelper;
        private static string _test = "";

        public UserControllerTest(TestApiFactory testApiFactory, ITestOutputHelper testOutputHelper)
        {
            _testApiFactory = testApiFactory;
            _testOutputHelper = testOutputHelper;
        }
        
        [Fact, TestPriority(1)]
        public async Task BWhenGetUser_ShouldReturnListOfSeedUsers()
        {
            _test += "Test B running";
            _testOutputHelper.WriteLine(_test);
            var expectedUser = new UserEntity
            {
                Id = 1,
                Name = "Lela",
                LastName = "Estes"
            };
            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User");
            var users = await result.Content.ReadFromJsonAsync<List<UserEntity>>();
            
            //Asserts
            users.Should().ContainEquivalentOf(expectedUser);
            _test += "Test B finished";
            _testOutputHelper.WriteLine(_test);
        }
        
        [Fact, TestPriority(2)]
        public async Task AWhenGetUser_ShouldReturnListOfSeedUsers()
        {
            _test += "Test A running";
            _testOutputHelper.WriteLine(_test);
            var expectedUser = new UserEntity
            {
                Id = 1,
                Name = "Lela",
                LastName = "Estes"
            };
            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User");
            var users = await result.Content.ReadFromJsonAsync<List<UserEntity>>();
            
            //Asserts
            users.Should().ContainEquivalentOf(expectedUser);
            _test += "Test A finished";
            _testOutputHelper.WriteLine(_test);
        }
        
        [Fact, TestPriority(3)]
        public async Task ZWhenGetUser_ShouldReturnListOfSeedUsers()
        {
            _test += "Test Z running";
            _testOutputHelper.WriteLine(_test);
            var expectedUser = new UserEntity
            {
                Id = 1,
                Name = "Lela",
                LastName = "Estes"
            };
            var client = _testApiFactory.CreateClient();
            var result = await client.GetAsync("User");
            var users = await result.Content.ReadFromJsonAsync<List<UserEntity>>();
            
            //Asserts
            users.Should().ContainEquivalentOf(expectedUser);
            _test += "Test Z finished";
            _testOutputHelper.WriteLine(_test);
        }
        
        // [Theory]
        // [InlineData(typeof(UserRepository))]
        // public async Task ShouldReturnX(Type repositoryType)
        // {
        //     var client = _testApiFactory.CreateClient();
        //     var expectedUser = new UserEntity
        //     {
        //         Id = 1,
        //         Name = "Lela",
        //         LastName = "Estes"
        //     };
        //     var userRepository = (IUserRepository)Activator.CreateInstance(repositoryType);
        //     var users = await userRepository.GetUsersAsync();
        //     var lalaUser = users.FirstOrDefault(u => u.Name.Contains("Lela"));
        //     
        //     //Asserts
        //     lalaUser.Should().BeEquivalentTo(expectedUser);
        // }
    }
}