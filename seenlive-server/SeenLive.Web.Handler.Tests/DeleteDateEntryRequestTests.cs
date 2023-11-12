using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.Web.Handler.Requests;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class DeleteDateEntryRequestTests
    {
        private readonly IArtistsRepository _artistsRepository = A.Fake<IArtistsRepository>();
        private readonly IDatesRepository _datesRepository = A.Fake<IDatesRepository>();

        [Fact]
        public async Task DeleteDateEntry_ArtistNotFound_DoesNotDeleteAnything()
        {
            DeleteDateEntryRequest request = new DeleteDateEntryRequest
                { UserId = "test", ArtistId = "test", DateId = "test" };

            DeleteDateEntryRequest.Handler handler = SetupHandler();
            A.CallTo(_artistsRepository).WithReturnType<IArtistEntity?>().Returns(null);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeFalse();
        }
        
        [Fact]
        public async Task DeleteDateEntry_DateNotFound_DoesNotDeleteAnything()
        {
            const string NoKnownDateId = "noKnownDate";
            
            DeleteDateEntryRequest request = new DeleteDateEntryRequest {ArtistId = "Artist-1", DateId = NoKnownDateId, UserId = "test"};
            DeleteDateEntryRequest.Handler handler = SetupHandler();
            FakeArtistWithoutDates(request.ArtistId);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeFalse();
            A.CallTo(() => _datesRepository.Remove(NoKnownDateId)).MustNotHaveHappened();
        }

        [Fact]
        public async Task DeleteDateEntry_DateFoundAndBelongsToArtistWithTwoDates_DeletesDateAndUpdatesArtist()
        {
            const string DateToDeleteId = "DateEntry-1";
            
            DeleteDateEntryRequest request = new DeleteDateEntryRequest { ArtistId = "Artist-1", DateId = DateToDeleteId, UserId = "test"};
            DeleteDateEntryRequest.Handler handler = SetupHandler();
            IArtistEntity artistEntity = FakeArtistWithTwoDates(DateToDeleteId, request.ArtistId);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeTrue();
            A.CallTo(() => _datesRepository.Remove(DateToDeleteId)).MustHaveHappenedOnceExactly().Then(
            A.CallTo(() => _artistsRepository.Update(artistEntity.Id, artistEntity)).MustHaveHappenedOnceExactly());
            artistEntity.DateEntryIDs.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteDateEntry_DateFoundAndBelongsToArtistWithNoOtherDate_DeletesDateAndDeletesArtist()
        {
            const string DateToDeleteId = "DateEntry-1";
            
            DeleteDateEntryRequest request = new DeleteDateEntryRequest { ArtistId = "Artist-1", DateId = DateToDeleteId, UserId = "test"};
            DeleteDateEntryRequest.Handler handler = SetupHandler();
            IArtistEntity artistEntity = FakeArtistWithSingleDate(DateToDeleteId, request.ArtistId);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeTrue();
            A.CallTo(() => _datesRepository.Remove(DateToDeleteId)).MustHaveHappenedOnceExactly().Then(
                A.CallTo(() => _artistsRepository.Remove(artistEntity.Id)).MustHaveHappenedOnceExactly());
        }

        private void FakeArtistWithoutDates(string artistId)
        {
            IArtistEntity artistEntity = A.Fake<IArtistEntity>();
            A.CallTo(() => artistEntity.Id).Returns("Artist-1");
            A.CallTo(() => _artistsRepository.Get(artistId)).Returns(artistEntity);
        }

        private IArtistEntity FakeArtistWithTwoDates(string dateToDeleteId, string artistId)
        {
            IArtistEntity artistEntity = A.Fake<IArtistEntity>();
            A.CallTo(() => artistEntity.Id).Returns("Artist-1");
            A.CallTo(() => artistEntity.DateEntryIDs).Returns(new List<string> { dateToDeleteId, "DateEntry-2" });
            A.CallTo(() => _artistsRepository.Get(artistId)).Returns(artistEntity);
            return artistEntity;
        }

        private IArtistEntity FakeArtistWithSingleDate(string dateToDeleteId, string artistId)
        {
            IArtistEntity artistEntity = A.Fake<IArtistEntity>();
            A.CallTo(() => artistEntity.Id).Returns("Artist-1");
            A.CallTo(() => artistEntity.DateEntryIDs).Returns(new List<string> { dateToDeleteId });
            A.CallTo(() => _artistsRepository.Get(artistId)).Returns(artistEntity);
            return artistEntity;
        }

        private DeleteDateEntryRequest.Handler SetupHandler()
        {
            A.CallTo(_artistsRepository).Where(call => call.Method.Name == nameof(_artistsRepository.Remove))
                .WithReturnType<bool>().Returns(true);
            
            A.CallTo(_datesRepository).Where(call => call.Method.Name == nameof(_datesRepository.Remove))
                .WithReturnType<bool>().Returns(true);
            
            return new DeleteDateEntryRequest.Handler(_artistsRepository, _datesRepository, A.Fake<IUserRepository>());
        }
    }
}