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
    public class DeleteDateEntryRequestTests
    {
        private IArtistService _artistService;
        private IDatesService _datesService;

        [Fact]
        public async Task DeleteDateEntry_ArtistNotFound_DoesNotDeleteAnything()
        {
            DeleteDateEntryRequest request = new DeleteDateEntryRequest();

            DeleteDateEntryRequest.Handler handler = SetupHandler();
            A.CallTo(_artistService).WithReturnType<IArtistEntry?>().Returns(null);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeFalse();
        }
        
        [Fact]
        public async Task DeleteDateEntry_DateNotFound_DoesNotDeleteAnything()
        {
            const string NoKnownDateId = "noKnownDate";
            
            DeleteDateEntryRequest request = new DeleteDateEntryRequest {ArtistId = "Artist-1", DateId = NoKnownDateId};
            DeleteDateEntryRequest.Handler handler = SetupHandler();
            FakeArtistWithoutDates(request.ArtistId);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeFalse();
            A.CallTo(() => _datesService.Remove(NoKnownDateId)).MustNotHaveHappened();
        }

        [Fact]
        public async Task DeleteDateEntry_DateFoundAndBelongsToArtistWithTwoDates_DeletesDateAndUpdatesArtist()
        {
            const string DateToDeleteId = "DateEntry-1";
            
            DeleteDateEntryRequest request = new DeleteDateEntryRequest { ArtistId = "Artist-1", DateId = DateToDeleteId };
            DeleteDateEntryRequest.Handler handler = SetupHandler();
            IArtistEntry artistEntry = FakeArtistWithTwoDates(DateToDeleteId, request.ArtistId);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeTrue();
            A.CallTo(() => _datesService.Remove(DateToDeleteId)).MustHaveHappenedOnceExactly().Then(
            A.CallTo(() => _artistService.Update(artistEntry.Id, artistEntry)).MustHaveHappenedOnceExactly());
            artistEntry.DateEntryIDs.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteDateEntry_DateFoundAndBelongsToArtistWithNoOtherDate_DeletesDateAndDeletesArtist()
        {
            const string DateToDeleteId = "DateEntry-1";
            
            DeleteDateEntryRequest request = new DeleteDateEntryRequest { ArtistId = "Artist-1", DateId = DateToDeleteId };
            DeleteDateEntryRequest.Handler handler = SetupHandler();
            IArtistEntry artistEntry = FakeArtistWithSingleDate(DateToDeleteId, request.ArtistId);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeTrue();
            A.CallTo(() => _datesService.Remove(DateToDeleteId)).MustHaveHappenedOnceExactly().Then(
                A.CallTo(() => _artistService.Remove(artistEntry.Id)).MustHaveHappenedOnceExactly());
        }

        private void FakeArtistWithoutDates(string artistId)
        {
            IArtistEntry artistEntry = A.Fake<IArtistEntry>();
            A.CallTo(() => artistEntry.Id).Returns("Artist-1");
            A.CallTo(() => _artistService.Get(artistId)).Returns(artistEntry);
        }

        private IArtistEntry FakeArtistWithTwoDates(string dateToDeleteId, string artistId)
        {
            IArtistEntry artistEntry = A.Fake<IArtistEntry>();
            A.CallTo(() => artistEntry.Id).Returns("Artist-1");
            A.CallTo(() => artistEntry.DateEntryIDs).Returns(new List<string> { dateToDeleteId, "DateEntry-2" });
            A.CallTo(() => _artistService.Get(artistId)).Returns(artistEntry);
            return artistEntry;
        }

        private IArtistEntry FakeArtistWithSingleDate(string dateToDeleteId, string artistId)
        {
            IArtistEntry artistEntry = A.Fake<IArtistEntry>();
            A.CallTo(() => artistEntry.Id).Returns("Artist-1");
            A.CallTo(() => artistEntry.DateEntryIDs).Returns(new List<string> { dateToDeleteId });
            A.CallTo(() => _artistService.Get(artistId)).Returns(artistEntry);
            return artistEntry;
        }

        private DeleteDateEntryRequest.Handler SetupHandler()
        {
            _artistService = A.Fake<IArtistService>();
            A.CallTo(_artistService).Where(call => call.Method.Name == nameof(_artistService.Remove))
                .WithReturnType<bool>().Returns(true);
            
            _datesService = A.Fake<IDatesService>();
            A.CallTo(_datesService).Where(call => call.Method.Name == nameof(_datesService.Remove))
                .WithReturnType<bool>().Returns(true);
            
            return new DeleteDateEntryRequest.Handler(_artistService, _datesService);
        }
    }
}