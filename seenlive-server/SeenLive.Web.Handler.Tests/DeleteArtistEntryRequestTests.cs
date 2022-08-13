using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Web.Handler.Bands;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class DeleteArtistEntryRequestTests
    {
        private readonly IArtistService _artistService;
        private readonly IDatesService _datesService;

        public DeleteArtistEntryRequestTests()
        {
            _artistService = A.Fake<IArtistService>();
            _datesService = A.Fake<IDatesService>();
        }

        [Fact]
        public async Task DeleteArtistEntry_EntryNotFound_NoEntryDeleted()
        {
            DeleteArtistEntryRequest request = new DeleteArtistEntryRequest { ArtistEntryId = "noKnownEntry" };
            DeleteArtistEntryRequest.Handler handler = SetupHandler();

            A.CallTo(_artistService).WithReturnType<IArtistEntry?>().Returns(null);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeFalse();
        }
        
        [Fact]
        public async Task DeleteArtistEntry_EntryFound_EntryDeletedWithDateEntries()
        {
            DeleteArtistEntryRequest request = new DeleteArtistEntryRequest { ArtistEntryId = "knownEntry" };
            DeleteArtistEntryRequest.Handler handler = SetupHandler();
            IArtistEntry artistEntry = A.Fake<IArtistEntry>();
            A.CallTo(() => artistEntry.DateEntryIDs).Returns(new List<string>{"dateEntry-1", "dateEntry-2"});
            A.CallTo(() => _artistService.Get(request.ArtistEntryId)).Returns(artistEntry);
            A.CallTo(() => _artistService.Remove(request.ArtistEntryId)).Returns(true);
            
            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeTrue();
            A.CallTo(_datesService)
                .Where(call => call.Method.Name == nameof(_datesService.Remove))
                .MustHaveHappenedTwiceExactly()
                .Then(A.CallTo(_artistService)
                    .Where(call => call.Method.Name == nameof(_artistService.Remove))
                    .MustHaveHappenedOnceExactly());
        }

        private DeleteArtistEntryRequest.Handler SetupHandler()
        {
            return new DeleteArtistEntryRequest.Handler(_artistService, _datesService);
        }
    }
}