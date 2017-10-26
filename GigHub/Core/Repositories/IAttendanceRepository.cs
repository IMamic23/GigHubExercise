using System.Collections.Generic;
using GigHub.Core.Dtos;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IAttendanceRepository
    {
        IEnumerable<Attendance> GetFutureAttendances(string userId);
        Attendance GetAttendance(int gigId, string userId);
        bool Any(int gigId, string userId);
        Attendance Add(Attendance attendance);
        void Delete(Attendance attendance);
    }
}