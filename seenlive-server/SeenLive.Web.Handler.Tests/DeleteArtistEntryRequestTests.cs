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
    public class DeleteArtistEntryRequestTests
    {
        private readonly IArtistsRepository _artistsRepository = A.Fake<IArtistsRepository>();
        private readonly IDatesRepository _datesRepository = A.Fake<IDatesRepository>();

        [Fact]
        public async Task DeleteArtistEntry_EntryNotFound_NoEntryDeleted()
        {
            DeleteArtistEntryRequest request = new DeleteArtistEntryRequest { ArtistEntryId = "noKnownEntry", UserId = "test"};
            DeleteArtistEntryRequest.Handler handler = SetupHandler();

            A.CallTo(_artistsRepository).WithReturnType<IArtistEntity?>().Returns(null);

            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeFalse();
        }
        
        [Fact]
        public async Task DeleteArtistEntry_EntryFound_EntryDeletedWithDateEntries()
        {
            DeleteArtistEntryRequest request = new DeleteArtistEntryRequest { ArtistEntryId = "knownEntry", UserId = "test"};
            DeleteArtistEntryRequest.Handler handler = SetupHandler();
            IArtistEntity artistEntity = A.Fake<IArtistEntity>();
            A.CallTo(() => artistEntity.DateEntryIDs).Returns(new List<string>{"dateEntry-1", "dateEntry-2"});
            A.CallTo(() => _artistsRepository.Get(request.ArtistEntryId)).Returns(artistEntity);
            A.CallTo(() => _artistsRepository.Remove(request.ArtistEntryId)).Returns(true);
            
            bool res = await handler.Handle(request, CancellationToken.None);

            res.Should().BeTrue();
            A.CallTo(_datesRepository)
                .Where(call => call.Method.Name == nameof(_datesRepository.Remove))
                .MustHaveHappenedTwiceExactly()
                .Then(A.CallTo(_artistsRepository)
                    .Where(call => call.Method.Name == nameof(_artistsRepository.Remove))
                    .MustHaveHappenedOnceExactly());
        }

        private DeleteArtistEntryRequest.Handler SetupHandler()
        {
            return new DeleteArtistEntryRequest.Handler(_artistsRepository, _datesRepository, A.Fake<IUserRepository>());
        }
    }
}