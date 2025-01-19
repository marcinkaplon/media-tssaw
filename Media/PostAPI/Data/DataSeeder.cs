using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using PostAPI.Models;

namespace PostAPI.Data
{
    public class DataSeeder
    {
        private readonly PostDbContext _dbContext;

        public DataSeeder(PostDbContext context)
        {
            this._dbContext = context;
        }

        public void Seed()
        {
            _dbContext.Database.EnsureCreated();
        }
    }
}
