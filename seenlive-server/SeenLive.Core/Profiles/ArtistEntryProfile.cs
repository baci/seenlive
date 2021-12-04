using AutoMapper;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.DTOs;
using System.Collections.Generic;
using System.Linq;
using SeenLive.Core.Abstractions;

namespace SeenLive.Core.Profiles
{
    public class ArtistEntryProfile : Profile
    {
        public ArtistEntryProfile()
        {
            CreateMap<IArtistEntry, ArtistResponseDTO>()
                .ForMember(dest => dest.ArtistName, config => config.MapFrom(src => src.ArtistName))
                .ForMember(dest => dest.DateEntries, config => config.MapFrom<ArtistEntryDateValueResolver>())
                //.ReverseMap()
                ;
        }
    }

    public sealed class ArtistEntryDateValueResolver : IValueResolver<IArtistEntry, ArtistResponseDTO, IList<DateEntryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IDatesService _datesService;

        public ArtistEntryDateValueResolver(IMapper mapper, IDatesService datesService)
        {
            _mapper = mapper;
            _datesService = datesService;
        }

        IList<DateEntryDTO> IValueResolver<IArtistEntry, ArtistResponseDTO, IList<DateEntryDTO>>.Resolve(
            IArtistEntry artistEntry, ArtistResponseDTO destination, IList<DateEntryDTO> destMember, ResolutionContext context
            )
        {
            return artistEntry.DateEntryIDs.Select(id => _mapper.Map<DateEntryDTO>(_datesService.Get(id))).ToList();
        }
    }
}
