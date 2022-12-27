using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestIntegrations;

namespace Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        public async Task<List<UserEntity>> GetUsersAsync()
        {
            var users = await _appDbContext.UserEntities.ToListAsync();

            return users;
        }
    }
}