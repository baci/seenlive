using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;

namespace SeenLive.Web.Handler.Bands
{
    public class DeleteArtistEntryRequest : IRequest<bool>
    {
        [Required] 
        public string ArtistEntryId { get; init; } = string.Empty;
        
        public class Handler : IRequestHandler<DeleteArtistEntryRequest, bool>
        {
            private readonly IArtistService _artistService;
            private readonly IDatesService _datesService;

            public Handler(IArtistService artistService, IDatesService datesService)
            {
                _artistService = artistService;
                _datesService = datesService;
            }

            public Task<bool> Handle(DeleteArtistEntryRequest request, CancellationToken cancellationToken)
            {
                IArtistEntry? artist = _artistService.Get(request.ArtistEntryId);
                if (artist == null)
                {
                    return Task.FromResult(false);
                }

                foreach (string dateId in artist.DateEntryIDs)
                {
                    _datesService.Remove(dateId);
                }
                
                return Task.FromResult(_artistService.Remove(request.ArtistEntryId));
            }
        }
    }
}