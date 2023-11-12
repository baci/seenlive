using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.Web.Handler.DTOs;

namespace SeenLive.Web.Handler.Requests
{
    public class GetArtistEntriesRequest : IRequest<IEnumerable<ArtistResponseDTO>>
    {
        public required string UserId { get; set; }
        
        public class Handler : IRequestHandler<GetArtistEntriesRequest, IEnumerable<ArtistResponseDTO>>
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

            public Task<IEnumerable<ArtistResponseDTO>> Handle(GetArtistEntriesRequest request, CancellationToken cancellationToken)
            {
                IUserEntity? userEntity = _userRepository.Get(request.UserId);
                if (userEntity == null)
                {
                    userEntity = _userRepository.Create(request.UserId, "TestUserName");
                    // TODO: implement authentication
                    //throw new ArgumentException("Invalid UserId");
                }
                IEnumerable<IArtistEntity> artistEntries = _artistsRepository.Get().Where(artistEntry => userEntity.ArtistEntryIDs.Contains(artistEntry.Id));

                IEnumerable<ArtistResponseDTO> artistEntryDtos = artistEntries.Select(artistEntry => artistEntry.Adapt<ArtistResponseDTO>() with
                {
                    DateEntries = artistEntry.DateEntryIDs.Select(dateEntry => _datesRepository.Get(dateEntry).Adapt<DateEntryDTO>()).ToList()
                });

                return Task.FromResult(artistEntryDtos);
            }
        }
    }
}