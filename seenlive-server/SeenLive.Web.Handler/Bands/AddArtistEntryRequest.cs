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
    public class AddArtistEntryRequest : IRequest<IEnumerable<ArtistResponseDTO>>
    {
        [Required]
        public ArtistCreationRequestDTO ArtistRequest { get; set; }
        
        
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
                IArtistEntry artistEntry = _artistService.Get().SingleOrDefault(entry => entry.ArtistName == request.ArtistRequest.ArtistName);
                IEnumerable<string> dateEntryIDs = CreateDateEntries(request.ArtistRequest.DateEntryRequests);

                if (artistEntry == null)
                {
                    // TODO geradeziehen
                    //artistEntry = new ArtistEntry(string.Empty, request.ArtistRequest.ArtistName, dateEntryIDs);
                    //_artistService.Create(artistEntry);
                }
                else
                {
                    artistEntry.AddDateEntries(dateEntryIDs);
                    _artistService.Update(artistEntry.Id, artistEntry);
                }

                return Task.FromResult(_artistService.Get().Select(entry => entry.Adapt<ArtistResponseDTO>()));
            }
            
            private IEnumerable<string> CreateDateEntries(IEnumerable<DateEntryCreationRequestDTO> requests)
            {
                throw new NotImplementedException();
                // return requests.Select(request =>
                // {
                //     IDateEntry newDateEntry = _datesService.Create(new DateEntry(request)); // TODO geradeziehen
                //     return newDateEntry?.Id;
                // });
            }
        }
    }
}