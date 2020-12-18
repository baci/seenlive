using AutoMapper;
using SeenLive.Server.DTOs;
using SeenLive.Server.Models;
using SeenLive.Server.Services;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.Server.Profiles
{
    public class ArtistEntryProfile : Profile
    {
        public ArtistEntryProfile()
        {
            CreateMap<ArtistEntry, ArtistResponseDTO>()
                .ForMember(dest => dest.ArtistName, config => config.MapFrom(src => src.ArtistName))
                .ForMember(dest => dest.DateEntries, config => config.MapFrom<ArtistEntryDateValueResolver>())
                //.ReverseMap()
                ;
        }
    }

    public class ArtistEntryDateValueResolver : IValueResolver<ArtistEntry, ArtistResponseDTO, IList<DateEntryDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IDatesService _datesService;

        public ArtistEntryDateValueResolver(IMapper mapper, IDatesService datesService)
        {
            _mapper = mapper;
            _datesService = datesService;
        }

        IList<DateEntryDTO> IValueResolver<ArtistEntry, ArtistResponseDTO, IList<DateEntryDTO>>.Resolve(
            ArtistEntry artistEntry, ArtistResponseDTO destination, IList<DateEntryDTO> destMember, ResolutionContext context
            )
        {
            return artistEntry.DateEntryIDs.Select(id => _mapper.Map<DateEntryDTO>(_datesService.Get(id))).ToList();
        }
    }
}
