using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestIntegrations.Entities;

namespace TestIntegrations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
    
        public UserController(AppDbContext context)
        {
            _context = context;
        }
    
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.UserEntities.ToListAsync();

            return Ok(users);
        }
    }
}