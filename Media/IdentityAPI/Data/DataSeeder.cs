using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using IdentityAPI.Models;

namespace IdentityAPI.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public DataSeeder(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public void Seed()
        {
            _dbContext.Database.EnsureCreated();
        }
    }
}
