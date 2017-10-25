using System.Linq;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
{
    public class FollowingRepository : IFollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool GetFollowing(string artistId, string userId)
        {
            return _context.Followings
                .Any(f => f.FolloweeId == artistId && f.FollowerId == userId);
        }
    }
}