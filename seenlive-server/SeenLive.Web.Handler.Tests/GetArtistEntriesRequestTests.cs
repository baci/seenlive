using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.DataAccess.Models;
using SeenLive.Web.Handler.DTOs;
using SeenLive.Web.Handler.Requests;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class GetArtistEntriesRequestTests
    {
        private readonly IArtistsRepository _artistsRepository = A.Fake<IArtistsRepository>();
        private readonly IDatesRepository _datesRepository = A.Fake<IDatesRepository>();
        private readonly IUserRepository _userRepository = A.Fake<IUserRepository>();

        [Theory]
        [MemberData(nameof(EntriesInDb))]
        public async Task GetArtistEntries_ForEntriesInDb_ReturnsEntriesWithTheirDateEntries(IArtistEntity[] artistEntries, IDateEntity[] dateEntries)
        {
            const string TestUserId = "test";
            GetArtistEntriesRequest request = new GetArtistEntriesRequest {UserId = TestUserId};

            GetArtistEntriesRequest.Handler handler = SetupHandler();
            
            A.CallTo(() => _userRepository.Get(TestUserId))
                .Returns(new UserEntity {Id = TestUserId, Username = TestUserId, ArtistEntryIDs = artistEntries.Select(x => x.Id).ToList()});

            A.CallTo(() => _artistsRepository.Get()).Returns(artistEntries);
            A.CallTo(() => _datesRepository.Get()).ReturnsNextFromSequence(dateEntries);

            IEnumerable<ArtistResponseDTO> response = (await handler.Handle(request, CancellationToken.None)).ToArray();

            response.Should().HaveCount(artistEntries.Length);
            response.Sum(dto => dto.DateEntries.Count).Should().Be(dateEntries.Length);
        }

        public static IEnumerable<object[]> EntriesInDb => 
            new []
            {
                new object[] { Array.Empty<IArtistEntity>(), Array.Empty<IDateEntity>() },
                new object[] { new [] { GetFakeArtistEntry(1) }, new [] { A.Fake<IDateEntity>() } },
                new object[] { 
                    new []{GetFakeArtistEntry(1), GetFakeArtistEntry(2)}, 
                    new []{A.Fake<IDateEntity>(), A.Fake<IDateEntity>(), A.Fake<IDateEntity>()} 
                }
            };

        private static IArtistEntity GetFakeArtistEntry(int amountOfDates)
        {
            IArtistEntity artistEntity = A.Fake<IArtistEntity>();
            A.CallTo(() => artistEntity.DateEntryIDs)
                .Returns(Enumerable.Range(0, amountOfDates)
                    .Select(dateId => $"DateEntry-{dateId}")
                    .ToList());

            return artistEntity;
        }

        private GetArtistEntriesRequest.Handler SetupHandler()
        {
            return new GetArtistEntriesRequest.Handler(_artistsRepository, _datesRepository, _userRepository);
        }
    }
}