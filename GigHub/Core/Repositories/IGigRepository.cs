using System.Collections.Generic;
using System.Threading.Tasks;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        Task<Gig> GetGig(int id);
        Gig GetGigWithAttendees(int gigId);
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        IEnumerable<Gig> GetUsersActiveFutureGigsWithGenre(string userId);
        void Add(Gig gig);
    }
}