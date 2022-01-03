using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.DTOs;

namespace SeenLive.Web.Handler.Bands
{
    public class AddArtistEntryRequest : IRequest
    {
        [Required]
        public ArtistCreationRequestDTO ArtistRequest { get; set; }
        
        
        public class Handler : IRequestHandler<AddArtistEntryRequest>
        {
            private readonly IArtistService _artistService;
            private readonly IDatesService _datesService;

            public Handler(IArtistService artistService, IDatesService datesService)
            {
                _artistService = artistService;
                _datesService = datesService;
            }

            public Task<Unit> Handle(AddArtistEntryRequest request, CancellationToken cancellationToken)
            {
                IArtistEntry artistEntry = _artistService.Get().SingleOrDefault(entry => entry.ArtistName == request.ArtistRequest.ArtistName);
                IEnumerable<string> dateEntryIDs = CreateDateEntries(request.ArtistRequest.DateEntryRequests);

                if (artistEntry == null)
                {
                    artistEntry = _artistService.Create(string.Empty, request.ArtistRequest.ArtistName, dateEntryIDs);
                }
                else
                {
                    artistEntry.AddDateEntries(dateEntryIDs);
                    _artistService.Update(artistEntry.Id, artistEntry);
                }

                return Task.FromResult(Unit.Value);
            }
            
            private IEnumerable<string> CreateDateEntries(IEnumerable<DateEntryCreationRequestDTO> requests)
            {
                return requests.Select(request =>
                {
                    IDateEntry newDateEntry = _datesService.Create(request.Date, request.Location, request.Remarks);
                    return newDateEntry?.Id;
                });
            }
        }
    }
}