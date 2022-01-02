using Mapster;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeenLive.Web.Handler.Bands
{
    public class GetArtistEntriesRequest : IRequest<IEnumerable<ArtistResponseDTO>>
    {
        public class Handler : IRequestHandler<AddArtistEntryRequest, IEnumerable<ArtistResponseDTO>>
        {
            private readonly IArtistService _artistService;
            private readonly IDatesService _datesService;

            public Handler(IArtistService artistService, IDatesService datesService)
            {
                _artistService = artistService;
                _datesService = datesService;
            }

            public Task<IEnumerable<ArtistResponseDTO>> Handle(AddArtistEntryRequest request, CancellationToken cancellationToken)
            {
                IEnumerable<IArtistEntry> artistEntries = _artistService.Get();

                IEnumerable<ArtistResponseDTO> artistEntryDtos = artistEntries.Select(artistEntry => artistEntry.Adapt<ArtistResponseDTO>() with
                {
                    DateEntries = artistEntry.DateEntryIDs.Select(dateEntry => _datesService.Get(dateEntry).Adapt<DateEntryDTO>()).ToList()
                });

                return Task.FromResult(artistEntryDtos);
            }
        }
    }
}