using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Web.Handler.Bands;
using SeenLive.Web.Handler.DTOs;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class GetArtistEntriesRequestTests
    {
        private readonly IArtistService _artistService;
        private readonly IDatesService _datesService;

        public GetArtistEntriesRequestTests()
        {
            _artistService = A.Fake<IArtistService>();
            _datesService = A.Fake<IDatesService>();
        }

        [Theory]
        [MemberData(nameof(EntriesInDb))]
        public async Task GetArtistEntries_ForEntriesInDb_ReturnsEntriesWithTheirDateEntries(IArtistEntry[] artistEntries, IDateEntry[] dateEntries)
        {
            GetArtistEntriesRequest request = new GetArtistEntriesRequest();

            GetArtistEntriesRequest.Handler handler = SetupHandler();

            A.CallTo(() => _artistService.Get()).Returns(artistEntries);
            A.CallTo(() => _datesService.Get()).ReturnsNextFromSequence(dateEntries);

            IEnumerable<ArtistResponseDTO> response = (await handler.Handle(request, CancellationToken.None)).ToArray();

            response.Should().HaveCount(artistEntries.Length);
            response.Sum(dto => dto.DateEntries.Count).Should().Be(dateEntries.Length);
        }

        public static IEnumerable<object[]> EntriesInDb => 
            new []
            {
                new object[] { Array.Empty<IArtistEntry>(), Array.Empty<IDateEntry>() },
                new object[] { new [] { GetFakeArtistEntry(1) }, new [] { A.Fake<IDateEntry>() } },
                new object[] { 
                    new []{GetFakeArtistEntry(1), GetFakeArtistEntry(2)}, 
                    new []{A.Fake<IDateEntry>(), A.Fake<IDateEntry>(), A.Fake<IDateEntry>()} 
                }
            };

        private static IArtistEntry GetFakeArtistEntry(int amountOfDates)
        {
            IArtistEntry artistEntry = A.Fake<IArtistEntry>();
            A.CallTo(() => artistEntry.DateEntryIDs)
                .Returns(Enumerable.Range(0, amountOfDates)
                    .Select(dateId => $"DateEntry-{dateId}")
                    .ToList());

            return artistEntry;
        }

        private GetArtistEntriesRequest.Handler SetupHandler()
        {
            return new GetArtistEntriesRequest.Handler(_artistService, _datesService);
        }
    }
}