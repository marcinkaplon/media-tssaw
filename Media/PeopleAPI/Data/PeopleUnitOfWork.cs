using System;
using PeopleAPI.Repositories;

namespace PeopleAPI.Data
{
    public class PeopleUnitOfWork :IPeopleUnitOfWork
    {
        private readonly PeopleDbContext _context;

        public IUserProfileRepository UserProfileRepository { get; }
        public IUserFollowsRepository UserFollowsRepository { get; }


        public PeopleUnitOfWork(
            PeopleDbContext context,
            IUserProfileRepository userProfileRepository,
            IUserFollowsRepository userFollowsRepository
            )
        {
            this._context = context;
            this.UserProfileRepository = userProfileRepository;
            this.UserFollowsRepository = userFollowsRepository;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
