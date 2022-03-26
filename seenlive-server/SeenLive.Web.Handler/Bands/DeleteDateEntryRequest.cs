using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public string DateId { get; init; } = string.Empty;

        [Required] 
        public string ArtistId { get; init; } = string.Empty;

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
                IArtistEntry? artist = _artistService.Get(request.ArtistId);
                if (artist == null)
                {
                    return Task.FromResult(false);
                }

                bool removeResult = RemoveDate(request.DateId, artist) && 
                                    UpdateOrDeleteArtist(artist);
                
                return Task.FromResult(removeResult);
            }

            private bool RemoveDate(string dateId, IArtistEntry artist)
            {
                return artist.DateEntryIDs.Remove(dateId) &&
                       _datesService.Remove(dateId);
            }

            private bool UpdateOrDeleteArtist(IArtistEntry artist)
            {
                if (artist.DateEntryIDs.Any())
                {
                    _artistService.Update(artist.Id, artist);

                    return true;
                }

                return _artistService.Remove(artist.Id);
            }
        }
    }
}