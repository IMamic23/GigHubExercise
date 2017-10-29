using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistence;
using GigHub.Persistence.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Tests.Persistance.Repositories
{
    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;
        private Mock<DbSet<Gig>> _mockGigs;
        private Mock<DbSet<Attendance>> _mockAttendances;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockGigs = new Mock<DbSet<Gig>>();
            _mockAttendances = new Mock<DbSet<Attendance>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);
            mockContext.SetupGet(c => c.Attendances).Returns(_mockAttendances.Object);

            _userId = "1";
            _repository = new GigRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(-1),
                ArtistId = _userId
            };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUsersActiveFutureGigsWithGenre(_userId);

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCanceled_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = _userId
            };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUsersActiveFutureGigsWithGenre(_userId);

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForADifferentArtist_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = _userId
            };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUsersActiveFutureGigsWithGenre(gig.ArtistId + "-");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForTheGvenArtinsAndInFuture_ShouldBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = _userId
            };

            _mockGigs.SetSource(new[] { gig });

            var gigs = _repository.GetUsersActiveFutureGigsWithGenre(gig.ArtistId);

            gigs.Should().Contain(gig);
        }

        [TestMethod]
        public void GetGigsUserAttending_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(-1)
            };
            var attendance = new Attendance()
            {
                Gig = gig,
                AttendeeId = _userId
            };

            _mockAttendances.SetSource(new[] {attendance});

            var gigs = _repository.GetGigsUserAttending(attendance.AttendeeId);

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserAttending_AttendanceForADifferentUser_ShouldNotBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1)
            };
            var attendance = new Attendance
            {
                Gig = gig, AttendeeId = _userId
            };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserAttending(attendance.AttendeeId + "-");

            gigs.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserAttending_UpcomingGigUserAttending_ShouldBeReturned()
        {
            var gig = new Gig()
            {
                DateTime = DateTime.Now.AddDays(1)
            };
            var attendance = new Attendance
            {
                Gig = gig, AttendeeId = _userId
            };

            _mockAttendances.SetSource(new[] { attendance });

            var gigs = _repository.GetGigsUserAttending(attendance.AttendeeId);

            gigs.Should().Contain(gig);
        }
    }
}
