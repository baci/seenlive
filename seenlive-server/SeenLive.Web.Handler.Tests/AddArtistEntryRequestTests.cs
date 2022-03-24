using FakeItEasy;
using FluentAssertions;
using SeenLive.Core.Abstractions;
using SeenLive.Core.DTOs;
using SeenLive.Web.Handler.Bands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SeenLive.Web.Handler.Tests
{
    public class AddArtistEntryRequestTests
    {
        [Fact]
        public async Task AddArtistEntry_MissingArgumentsInRequest_ThrowsInvalidArguments()
        {
            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO());

            AddArtistEntryRequest.Handler handler = new(A.Fake<IArtistService>(), A.Fake<IDatesService>());

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().ThrowAsync<InvalidArgumentsException>();
        }

        [Fact]
        public async Task AddArtistEntry_EmptyDateEntries_ThrowsInvalidArguments()
        {
            AddArtistEntryRequest request = new(new ArtistCreationRequestDTO() { ArtistName = "test", DateEntryRequests = new List<DateEntryCreationRequestDTO>() });

            AddArtistEntryRequest.Handler handler = new(A.Fake<IArtistService>(), A.Fake<IDatesService>());

            Func<Task> func = async () => await handler.Handle(request, CancellationToken.None);

            await func.Should().ThrowAsync<InvalidArgumentsException>();
        }
    }
}
