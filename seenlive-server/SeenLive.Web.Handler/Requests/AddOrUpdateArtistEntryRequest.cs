using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Entities;
using SeenLive.Web.Handler.DTOs;

namespace SeenLive.Web.Handler.Requests
{
    public class AddOrUpdateArtistEntryRequest : IRequest
    {
        public ArtistCreationRequestDTO ArtistRequest { get; }

        public AddOrUpdateArtistEntryRequest(ArtistCreationRequestDTO artistRequest)
        {
            ArtistRequest = artistRequest;
        }

        public class Handler : IRequestHandler<AddOrUpdateArtistEntryRequest>
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

            public Task Handle(AddOrUpdateArtistEntryRequest request, CancellationToken cancellationToken)
            {
                if (!ValidateRequestData(request))
                {
                    throw new ArgumentException("Invalid ArtistCreationRequestDTO");
                }
                
                IUserEntity? userEntity = _userRepository.Get(request.ArtistRequest.UserId);
                if (userEntity == null)
                {
                    throw new ArgumentException("Invalid UserId");
                }

                IArtistEntity? artistEntry = _artistsRepository.Get().SingleOrDefault(entry => 
                    entry.ArtistName == request.ArtistRequest.ArtistName && userEntity.ArtistEntryIDs.Contains(entry.Id));
                IEnumerable<string> dateEntryIDs = CreateDateEntries(request.ArtistRequest.DateEntryRequests);

                if (artistEntry == null)
                {
                    var artistEntity = _artistsRepository.Create(string.Empty, request.ArtistRequest.ArtistName, dateEntryIDs);
                    userEntity.ArtistEntryIDs.Add(artistEntity.Id);
                    _userRepository.Update(userEntity.Id, userEntity);
                }
                else
                {
                    artistEntry.AddDateEntries(dateEntryIDs);
                    _artistsRepository.Update(artistEntry.Id, artistEntry);
                }

                return Task.FromResult(Unit.Value);
            }

            private static bool ValidateRequestData(AddOrUpdateArtistEntryRequest request)
            {
                return request.ArtistRequest.DateEntryRequests.Any()
                    && request.ArtistRequest.DateEntryRequests.All(dateEntry => !string.IsNullOrWhiteSpace(dateEntry.Date))
                    && !string.IsNullOrWhiteSpace(request.ArtistRequest.ArtistName);
            }

            private IEnumerable<string> CreateDateEntries(IEnumerable<DateEntryCreationRequestDTO> requests)
            {
                return requests.Select(request =>
                {
                    IDateEntity newDateEntity = _datesRepository.Create(request.Date, request.Location, request.Remarks);
                    return newDateEntity.Id;
                }).ToArray();
            }
        }
    }
}