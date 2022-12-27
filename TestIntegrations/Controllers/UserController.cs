using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestIntegrations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;


        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetUsersAsync();

            return Ok(users);
        }
    }
}