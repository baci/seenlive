using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;

namespace SeenLive.Web.Handler.Bands
{
    public class DeleteDateEntryRequest : IRequest<bool>
    {
        [Required]
        public string DateId { get; set; }

        [Required]
        public string ArtistId { get; set; }

        public class Handler : IRequestHandler<DeleteDateEntryRequest, bool>
        {
            private readonly IArtistService _artistService;
            private readonly IDatesService _datesService;

            public Handler(IArtistService artistService, IDatesService datesService)
            {
                _artistService = artistService;
                _datesService = datesService;
            }

            public Task<bool> Handle(DeleteDateEntryRequest request, CancellationToken cancellationToken)
            {
                
                IArtistEntry artist = _artistService.Get(request.ArtistId);
                bool removeResult = _datesService.Remove(request.DateId);
                artist.DateEntryIDs.Remove(request.DateId);

                if (artist.DateEntryIDs.Count > 0)
                {                    
                    _artistService.Update(request.ArtistId, artist);

                    return Task.FromResult(removeResult);
                }

                return Task.FromResult(_artistService.Remove(artist.Id));
            }
        }
    }
}