using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Web.Handler.DTOs;
using SeenLive.Web.Handler.Exceptions;

namespace SeenLive.Web.Handler.Bands
{
    public class AddArtistEntryRequest : IRequest
    {
        public ArtistCreationRequestDTO ArtistRequest { get; }

        public AddArtistEntryRequest(ArtistCreationRequestDTO artistRequest)
        {
            ArtistRequest = artistRequest;
        }

        public class Handler : IRequestHandler<AddArtistEntryRequest>
        {
            private readonly IArtistService _artistService;
            private readonly IDatesService _datesService;

            public Handler(IArtistService artistService, IDatesService datesService)
            {
                _artistService = artistService;
                _datesService = datesService;
            }

            public Task Handle(AddArtistEntryRequest request, CancellationToken cancellationToken)
            {
                if (!ValidateRequestData(request))
                {
                    throw new InvalidArgumentException("Invalid ArtistCreationRequestDTO");
                }

                IArtistEntry? artistEntry = _artistService.Get().SingleOrDefault(entry => entry.ArtistName == request.ArtistRequest.ArtistName);
                IEnumerable<string> dateEntryIDs = CreateDateEntries(request.ArtistRequest.DateEntryRequests);

                if (artistEntry == null)
                {
                    _artistService.Create(string.Empty, request.ArtistRequest.ArtistName, dateEntryIDs);
                }
                else
                {
                    artistEntry.AddDateEntries(dateEntryIDs);
                    _artistService.Update(artistEntry.Id, artistEntry);
                }

                return Task.FromResult(Unit.Value);
            }

            private static bool ValidateRequestData(AddArtistEntryRequest request)
            {
                return request.ArtistRequest.DateEntryRequests.Any()
                    && request.ArtistRequest.DateEntryRequests.All(dateEntry => !string.IsNullOrWhiteSpace(dateEntry.Date))
                    && !string.IsNullOrWhiteSpace(request.ArtistRequest.ArtistName);
            }

            private IEnumerable<string> CreateDateEntries(IEnumerable<DateEntryCreationRequestDTO> requests)
            {
                return requests.Select(request =>
                {
                    IDateEntry newDateEntry = _datesService.Create(request.Date, request.Location, request.Remarks);
                    return newDateEntry.Id;
                }).ToArray();
            }
        }
    }
}