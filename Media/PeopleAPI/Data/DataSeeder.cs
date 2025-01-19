using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using PeopleAPI.Models;

namespace PeopleAPI.Data
{
    public class DataSeeder
    {
        private readonly PeopleDbContext _dbContext;

        public DataSeeder(PeopleDbContext context)
        {
            this._dbContext = context;
        }

        public void Seed()
        {
            _dbContext.Database.EnsureCreated();
        }
    }
}
