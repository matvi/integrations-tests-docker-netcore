using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using TestIntegrations;

namespace Infraestructure.Data
{
    public static class Seed
    {
        public static async Task SeedUsersAsync(AppDbContext appDbContext)
        {
            if (await appDbContext.UserEntities.AnyAsync())
            {
                return;
            }

            var userData = await File.ReadAllTextAsync("Data/UserSeed.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var users =  JsonSerializer.Deserialize<List<UserEntity>>(userData, options);

            if (users is null)
            {
                return;
            }

            await appDbContext.UserEntities.AddRangeAsync(users);

            await appDbContext.SaveChangesAsync();
        }
        
        public static async Task SeedUsers(AppDbContext appDbContext)
        {
            if (appDbContext.UserEntities.Any())
            {
                return;
            }

            var userData = await File.ReadAllTextAsync("Data/UserSeed.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var users =  JsonSerializer.Deserialize<List<UserEntity>>(userData, options);

            if (users is null)
            {
                return;
            }

            appDbContext.UserEntities.AddRange(users);

            appDbContext.SaveChanges();
        }
    }
}