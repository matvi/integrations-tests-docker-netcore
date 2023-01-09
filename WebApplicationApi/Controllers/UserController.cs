using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace TestIntegrations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(
            IUserRepository userRepository,
            IHttpClientFactory httpClientFactory)
        {
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
        }
    
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();

            return Ok(users);
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CallExternalAPI()
        {
            var username = "myuser";
            var password = "mypass";
            var queryString = new Dictionary<string, string> { { "environment", "test" } };
            var queryStringEnvironmentParameter = QueryHelpers.AddQueryString("/rest-api/v1/system/apikey", queryString);
            using var request = new HttpRequestMessage(HttpMethod.Post, queryStringEnvironmentParameter)
            {
                Content = JsonContent.Create(new { username, password })
            };

            var client = _httpClientFactory.CreateClient("myClient");
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                return Ok();
            }
            
            return BadRequest();
        }
    }
}