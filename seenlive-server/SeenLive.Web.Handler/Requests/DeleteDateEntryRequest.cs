using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;

namespace SeenLive.Web.Handler.Requests
{
    public class DeleteDateEntryRequest : IRequest<bool>
    {
        public required string UserId { get; set; }
        public required string DateId { get; init; }
        public required string ArtistId { get; init; }

        public class Handler : IRequestHandler<DeleteDateEntryRequest, bool>
        {
            private readonly IArtistsRepository _artistsRepository;
            private readonly IDatesRepository _datesRepository;
            private readonly IUserRepository _usersRepository;

            public Handler(IArtistsRepository artistsRepository, IDatesRepository datesRepository, IUserRepository usersRepository)
            {
                _artistsRepository = artistsRepository;
                _datesRepository = datesRepository;
                _usersRepository = usersRepository;
            }

            public Task<bool> Handle(DeleteDateEntryRequest request, CancellationToken cancellationToken)
            {
                IArtistEntity? artist = _artistsRepository.Get(request.ArtistId);
                if (artist == null)
                {
                    return Task.FromResult(false);
                }
                
                IUserEntity? userEntity = _usersRepository.Get(request.UserId);
                if (userEntity == null)
                {
                    throw new ArgumentException("Invalid UserId");
                }
                
                bool removeResult = RemoveDate(request.DateId, artist) && 
                                    UpdateOrDeleteArtist(artist, userEntity);
                
                return Task.FromResult(removeResult);
            }

            private bool RemoveDate(string dateId, IArtistEntity artist)
            {
                return artist.DateEntryIDs.Remove(dateId) &&
                       _datesRepository.Remove(dateId);
            }

            private bool UpdateOrDeleteArtist(IArtistEntity artist, IUserEntity userEntity)
            {
                if (artist.DateEntryIDs.Any())
                {
                    _artistsRepository.Update(artist.Id, artist);

                    return true;
                }
                
                userEntity.ArtistEntryIDs.Remove(artist.Id);
                _usersRepository.Update(userEntity.Id, userEntity);
                
                return _artistsRepository.Remove(artist.Id);
            }
        }
    }
}