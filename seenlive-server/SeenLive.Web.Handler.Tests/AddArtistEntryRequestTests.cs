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
using SeenLive.Web.Handler.DTOs;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class AddArtistEntryRequestTests
    {
        private readonly IArtistService _artistService;
        private readonly IDatesService _datesService;

        public static IEnumerable<object[]> InvalidArgumentsData => 
            new []
            {
                new object[] { null!, null! },
                new object[] { "test", new List<DateEntryCreationRequestDTO>() },
                new object[] { "test", new List<DateEntryCreationRequestDTO> { new DateEntryCreationRequestDTO() } }
            };

        public AddArtistEntryRequestTests()
        {
            _artistService = A.Fake<IArtistService>();
            _datesService = A.Fake<IDatesService>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsData))]
        public async Task AddArtistEntry_MissingArgumentsInRequest_ThrowsInvalidArgument(string artistName, 
            List<DateEntryCreationRequestDTO> dateEntries)
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();

            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO
            {
                ArtistName = artistName, 
                DateEntryRequests = dateEntries
            });

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().ThrowAsync<InvalidArgumentException>();
        }

        [Fact]
        public async Task AddArtistEntry_SingleDateEntryWithValidDate_DoesNotThrow()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();

            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO
            {
                ArtistName = "test",
                DateEntryRequests = new List<DateEntryCreationRequestDTO>
                {
                    new DateEntryCreationRequestDTO { Date = "myDate" }
                }
            });

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().NotThrowAsync();
        }

        [Fact]
        public async Task AddArtistEntry_ArtistNotFoundInDb_ArtistGetsCreatedInDb()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();
            AddArtistEntryRequest request = CreateValidRequestWithOneDate();

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(_datesService).Where(call => call.Method.Name == nameof(_datesService.Create))
                .MustHaveHappened(request.ArtistRequest!.DateEntryRequests.Count(), Times.Exactly);
            A.CallTo(_artistService).Where(call => call.Method.Name == nameof(_artistService.Create))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddArtistEntry_ArtistFoundInDb_ArtistGetsUpdatedInDbWithDates()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();
            AddArtistEntryRequest request = CreateValidRequestWithOneDate();

            IArtistEntry artistEntry = SetupArtistEntryInDb(request);

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(_datesService).Where(call => call.Method.Name == nameof(_datesService.Create))
                .MustHaveHappened(request.ArtistRequest!.DateEntryRequests.Count(), Times.Exactly)
                .Then(A.CallTo(artistEntry).Where(call => call.Method.Name == nameof(artistEntry.AddDateEntries))
                .MustHaveHappenedOnceExactly())
                .Then(A.CallTo(_artistService).Where(call => call.Method.Name == nameof(_artistService.Update))
                .MustHaveHappenedOnceExactly());
        }

        private IArtistEntry SetupArtistEntryInDb(AddArtistEntryRequest request)
        {
            IArtistEntry artistEntry = A.Fake<IArtistEntry>();
            A.CallTo(() => artistEntry.ArtistName)!.Returns(request.ArtistRequest!.ArtistName);
            A.CallTo(() => _artistService.Get()).Returns(new[] { artistEntry });
            
            return artistEntry;
        }

        [Fact]
        public async Task AddArtistEntry_MultipleEqualDates_DatesStillGetCreatedInDb()
        {
            AddArtistEntryRequest.Handler handler = SetupHandler();
            AddArtistEntryRequest request = CreateValidRequestWithTwoEqualDates();

            await handler.Handle(request, CancellationToken.None);

            A.CallTo(_datesService)
                .Where(call => call.Method.Name == nameof(_datesService.Create))
                .MustHaveHappened(request.ArtistRequest!.DateEntryRequests.Count(), Times.Exactly);
        }

        private static AddArtistEntryRequest CreateValidRequestWithOneDate()
        {
            return new AddArtistEntryRequest(new ArtistCreationRequestDTO
            { 
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

        private static AddArtistEntryRequest CreateValidRequestWithTwoEqualDates()
        {
            return new AddArtistEntryRequest(new ArtistCreationRequestDTO
            {
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

        private AddArtistEntryRequest.Handler SetupHandler()
        {
            A.CallTo(_artistService)
                .Where(call => call.Method.Name == nameof(_artistService.Create))
                .WithReturnType<IArtistEntry>()
                .Returns(A.Fake<IArtistEntry>());

            A.CallTo(_datesService)
                .Where(call => call.Method.Name == nameof(_datesService.Create))
                .WithReturnType<IDateEntry>()
                .Returns(A.Fake<IDateEntry>());

            return new(_artistService, _datesService);
        }
    }
}
