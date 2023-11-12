using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.DataAccess.Models;
using SeenLive.Web.Handler.DTOs;
using SeenLive.Web.Handler.Requests;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class AddArtistEntryRequestTests
    {
        private readonly IArtistsRepository _artistsRepository = A.Fake<IArtistsRepository>();
        private readonly IDatesRepository _datesRepository = A.Fake<IDatesRepository>();
        private readonly IUserRepository _userRepository = A.Fake<IUserRepository>();

        private const string TestUserId = "TestUserId";

        [Fact]
        public async Task AddArtistEntry_SingleDateEntryWithValidDate_DoesNotThrow()
        {
            AddOrUpdateArtistEntryRequest.Handler handler = SetupHandler();

            AddOrUpdateArtistEntryRequest request = new(new ArtistCreationRequestDTO
            {
                UserId = TestUserId,
                ArtistName = "test",
                DateEntryRequests = new List<DateEntryCreationRequestDTO>
                {
                    new DateEntryCreationRequestDTO { Date = "myDate", Location = string.Empty, Remarks = string.Empty }
                }
            });

            A.CallTo(() => _userRepository.Get(TestUserId))
                .Returns(new UserEntity { Id = TestUserId, Username = TestUserId, ArtistEntryIDs = new List<string>{request.ArtistRequest.ArtistName} });

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddArtistEntry_ArtistNotFoundInDb_ArtistGetsCreatedInDb()
        {
            AddOrUpdateArtistEntryRequest.Handler handler = SetupHandler();
            AddOrUpdateArtistEntryRequest request = CreateValidRequestWithOneDate();
            
            A.CallTo(() => _userRepository.Get(TestUserId))
                .Returns(new UserEntity { Id = TestUserId, Username = TestUserId, ArtistEntryIDs = new List<string>{request.ArtistRequest.ArtistName} });

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(() => _datesRepository.Create(A<string>._, A<string>._, A<string>._))
                .MustHaveHappened(request.ArtistRequest.DateEntryRequests.Count(), Times.Exactly);
            A.CallTo(() => _artistsRepository.Create(A<string>._, A<string>._, A<IEnumerable<string>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddArtistEntry_ArtistFoundInDb_ArtistGetsUpdatedInDbWithDates()
        {
            AddOrUpdateArtistEntryRequest.Handler handler = SetupHandler();
            AddOrUpdateArtistEntryRequest request = CreateValidRequestWithOneDate();

            A.CallTo(() => _userRepository.Get(TestUserId))
                .Returns(new UserEntity { Id = TestUserId, Username = TestUserId, ArtistEntryIDs = new List<string>{request.ArtistRequest.ArtistName} });

            IArtistEntity artistEntity = SetupArtistEntryInDb(request);

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(() => _datesRepository.Create(A<string>._, A<string>._, A<string>._))
                .MustHaveHappened(request.ArtistRequest.DateEntryRequests.Count(), Times.Exactly)
                .Then(A.CallTo(artistEntity).Where(call => call.Method.Name == nameof(artistEntity.AddDateEntries))
                .MustHaveHappenedOnceExactly())
                .Then(A.CallTo(_artistsRepository).Where(call => call.Method.Name == nameof(_artistsRepository.Update))
                .MustHaveHappenedOnceExactly());
        }

        private IArtistEntity SetupArtistEntryInDb(AddOrUpdateArtistEntryRequest request)
        {
            IArtistEntity artistEntity = A.Fake<IArtistEntity>();
            A.CallTo(() => artistEntity.ArtistName).Returns(request.ArtistRequest.ArtistName);
            A.CallTo(() => artistEntity.Id).Returns(request.ArtistRequest.ArtistName);
            A.CallTo(() => _artistsRepository.Get()).Returns(new[] { artistEntity });
            
            return artistEntity;
        }

        [Fact]
        public async Task AddArtistEntry_MultipleEqualDates_DatesStillGetCreatedInDb()
        {
            AddOrUpdateArtistEntryRequest.Handler handler = SetupHandler();
            
            AddOrUpdateArtistEntryRequest request = CreateValidRequestWithTwoEqualDates();
            A.CallTo(() => _userRepository.Get(TestUserId))
                .Returns(new UserEntity { Id = TestUserId, Username = TestUserId, ArtistEntryIDs = new List<string>{request.ArtistRequest.ArtistName} });

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(_datesRepository)
                .Where(call => call.Method.Name == nameof(_datesRepository.Create))
                .MustHaveHappened(request.ArtistRequest.DateEntryRequests.Count(), Times.Exactly);
        }

        private static AddOrUpdateArtistEntryRequest CreateValidRequestWithOneDate()
        {
            return new AddOrUpdateArtistEntryRequest(new ArtistCreationRequestDTO
            { 
                UserId = TestUserId,
                ArtistName = "TestArtist", 
                DateEntryRequests = new List<DateEntryCreationRequestDTO>
                { 
                    new DateEntryCreationRequestDTO
                    { 
                        Date = "TestDate", 
                        Location = "TestLocation", 
                        Remarks = "TestRemarks" 
                    } 
                } 
            });
        }

        private static AddOrUpdateArtistEntryRequest CreateValidRequestWithTwoEqualDates()
        {
            return new AddOrUpdateArtistEntryRequest(new ArtistCreationRequestDTO
            {
                UserId = TestUserId,
                ArtistName = "TestArtist",
                DateEntryRequests = new List<DateEntryCreationRequestDTO>
                {
                    new DateEntryCreationRequestDTO
                    {
                        Date = "TestDate",
                        Location = "TestLocation",
                        Remarks = "TestRemarks"
                    },
                    new DateEntryCreationRequestDTO
                    {
                        Date = "TestDate",
                        Location = "TestLocation",
                        Remarks = "TestRemarks"
                    }
                }
            });
        }

        private AddOrUpdateArtistEntryRequest.Handler SetupHandler()
        {
            A.CallTo(_artistsRepository)
                .Where(call => call.Method.Name == nameof(_artistsRepository.Create))
                .WithReturnType<IArtistEntity>()
                .Returns(A.Fake<IArtistEntity>());

            A.CallTo(_datesRepository)
                .Where(call => call.Method.Name == nameof(_datesRepository.Create))
                .WithReturnType<IDateEntity>()
                .Returns(A.Fake<IDateEntity>());
            
            return new AddOrUpdateArtistEntryRequest.Handler(_artistsRepository, _datesRepository, _userRepository);
        }
    }
}
