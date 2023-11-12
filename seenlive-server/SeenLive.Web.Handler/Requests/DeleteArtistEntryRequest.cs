using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;

namespace SeenLive.Web.Handler.Requests
{
    public class DeleteArtistEntryRequest : IRequest<bool>
    {
        public required string UserId { get; set; }
        public required string ArtistEntryId { get; init; }
        
        public class Handler : IRequestHandler<DeleteArtistEntryRequest, bool>
        {
            private readonly IArtistsRepository _artistsRepository;
            private readonly IDatesRepository _datesRepository;
            private readonly IUserRepository _userRepository;

            public Handler(IArtistsRepository artistsRepository, IDatesRepository datesRepository, IUserRepository userRepository)
            {
                _artistsRepository = artistsRepository;
                _datesRepository = datesRepository;
                _userRepository = userRepository;
            }

            public Task<bool> Handle(DeleteArtistEntryRequest request, CancellationToken cancellationToken)
            {
                IArtistEntity? artist = _artistsRepository.Get(request.ArtistEntryId);
                if (artist == null)
                {
                    return Task.FromResult(false);
                }
                
                IUserEntity? userEntity = _userRepository.Get(request.UserId);
                if (userEntity == null)
                {
                    throw new ArgumentException("Invalid UserId");
                }

                foreach (string dateId in artist.DateEntryIDs)
                {
                    _datesRepository.Remove(dateId);
                }
                
                userEntity.ArtistEntryIDs.Remove(artist.Id);
                _userRepository.Update(userEntity.Id, userEntity);
                
                return Task.FromResult(_artistsRepository.Remove(request.ArtistEntryId));
            }
        }
    }
}