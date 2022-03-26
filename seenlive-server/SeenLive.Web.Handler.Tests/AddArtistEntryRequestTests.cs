using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.DTOs;
using SeenLive.Web.Handler.Bands;
using SeenLive.Web.Handler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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
            AddArtistEntryRequest request = CreateValidRequestWithOneDate(); // TODO as theory with more or less date entries

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(_datesService).Where(call => call.Method.Name == nameof(_datesService.Create))
                .MustHaveHappened(request.ArtistRequest.DateEntryRequests.Count(), Times.Exactly);
            A.CallTo(_artistService).Where(call => call.Method.Name == nameof(_artistService.Create))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddArtistEntry_ArtistFoundInDb_ArtistGetsUpdatedInDbWithDates()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();
            AddArtistEntryRequest request = CreateValidRequestWithOneDate(); // TODO as theory with more or less date entries

            IArtistEntry artistEntry = A.Fake<IArtistEntry>();
            A.CallTo(() => artistEntry.ArtistName).Returns(request.ArtistRequest.ArtistName);
            A.CallTo(() => _artistService.Get()).Returns(new[] { artistEntry });

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(_datesService).Where(call => call.Method.Name == nameof(_datesService.Create))
                .MustHaveHappened(request.ArtistRequest.DateEntryRequests.Count(), Times.Exactly)
                .Then(
            A.CallTo(artistEntry).Where(call => call.Method.Name == nameof(artistEntry.AddDateEntries))
                .MustHaveHappenedOnceExactly())
                .Then(
                A.CallTo(_artistService).Where(call => call.Method.Name == nameof(_artistService.Update))
                .MustHaveHappenedOnceExactly());
        }

        [Fact]
        public async Task AddArtistEntry_MultipleEqualDates_DatesStillGetCreatedInDb()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();
            AddArtistEntryRequest request = CreateValidRequestWithTwoEqualDates();

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(_datesService).Where(call => call.Method.Name == nameof(_datesService.Create))
                .MustHaveHappened(request.ArtistRequest.DateEntryRequests.Count(), Times.Exactly);
        }        

        private static AddArtistEntryRequest CreateValidRequestWithOneDate()
        {
            return new(new ArtistCreationRequestDTO() { 
                ArtistName = "TestArtist", 
                DateEntryRequests = new List<DateEntryCreationRequestDTO>() { 
                    new DateEntryCreationRequestDTO() { 
                        Date = "TestDate", 
                        Location = "TestLocation", 
                        Remarks = "TestRemarks" 
                    } 
                } 
            });
        }

        private static AddArtistEntryRequest CreateValidRequestWithTwoEqualDates()
        {
            return new(new ArtistCreationRequestDTO()
            {
                ArtistName = "TestArtist",
                DateEntryRequests = new List<DateEntryCreationRequestDTO>() {
                    new DateEntryCreationRequestDTO() {
                        Date = "TestDate",
                        Location = "TestLocation",
                        Remarks = "TestRemarks"
                    },
                    new DateEntryCreationRequestDTO() {
                        Date = "TestDate",
                        Location = "TestLocation",
                        Remarks = "TestRemarks"
                    }
                }
            });
        }

        private AddArtistEntryRequest.Handler SetupHandler()
        {
            _artistService = A.Fake<IArtistService>();
            A.CallTo(_artistService).Where(call => call.Method.Name == nameof(_artistService.Create))
                .WithReturnType<IArtistEntry>().Returns(A.Fake<IArtistEntry>());

            _datesService = A.Fake<IDatesService>();
            A.CallTo(_datesService).Where(call => call.Method.Name == nameof(_datesService.Create))
                .WithReturnType<IDateEntry>().Returns(A.Fake<IDateEntry>());

            return new(_artistService, _datesService);
        }
    }
}
