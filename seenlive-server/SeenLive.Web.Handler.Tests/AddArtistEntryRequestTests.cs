using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using SeenLive.Core.DTOs;
using SeenLive.Web.Handler.Bands;
using SeenLive.Web.Handler.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class AddArtistEntryRequestTests
    {
        private IArtistService _artistService;
        private IDatesService _datesService;

        // TODO merge all tests throwing an InvalidArgumentsException as a single theory test
        [Fact]
        public async Task AddArtistEntry_MissingArgumentsInRequest_ThrowsInvalidArgument()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();

            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO());

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().ThrowAsync<InvalidArgumentException>();
        }

        [Fact]
        public async Task AddArtistEntry_EmptyDateEntries_ThrowsInvalidArgument()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();

            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO() { ArtistName = "test", DateEntryRequests = new List<DateEntryCreationRequestDTO>() });

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().ThrowAsync<InvalidArgumentException>();
        }

        [Fact]
        public async Task AddArtistEntry_SingleDateEntryWithoutValidDate_ThrowsInvalidArgument()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();

            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO() { ArtistName = "test", DateEntryRequests = new List<DateEntryCreationRequestDTO>() { new DateEntryCreationRequestDTO() } });

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().ThrowAsync<InvalidArgumentException>();
        }

        [Fact]
        public async Task AddArtistEntry_SingleDateEntryWithValidDate_DoesNotThrow()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();

            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO() { ArtistName = "test", DateEntryRequests = new List<DateEntryCreationRequestDTO>() { new DateEntryCreationRequestDTO() { Date = "myDate" } } });

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddArtistEntry_ArtistNotFoundInDb_ArtistGetsCreatedInDb()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();

            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO() { ArtistName = "test", DateEntryRequests = new List<DateEntryCreationRequestDTO>() { new DateEntryCreationRequestDTO() { Date = "myDate" } } });

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            A.CallTo(_artistService).Where(call => call.Method.Name == nameof(_artistService.Create)).MustHaveHappenedOnceExactly();
        }

        private AddArtistEntryRequest.Handler SetupHandler()
        {
            _artistService = A.Fake<IArtistService>();
            _datesService = A.Fake<IDatesService>();

            return new(_artistService, _datesService);
        }
    }
}
