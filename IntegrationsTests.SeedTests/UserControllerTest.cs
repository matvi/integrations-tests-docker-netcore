using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using FluentAssertions;
using Infraestructure.Repositories;
using Xunit;

namespace IntegrationsTests.SeedTests
{
    public class UserControllerTest : IClassFixture<TestApiFactory>
    {
        private readonly TestApiFactory _testApiFactory;

        public UserControllerTest(TestApiFactory testApiFactory)
        {
            _testApiFactory = testApiFactory;
        }
        
        [Fact]
        public async Task WhenGetUser_ShouldReturnListOfSeedUsers()
        {
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
        }
    }
}