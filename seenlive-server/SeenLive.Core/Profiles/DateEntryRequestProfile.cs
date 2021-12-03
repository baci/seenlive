using AutoMapper;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.DTOs;

namespace SeenLive.Core.Profiles
{
    public class DateEntryRequestProfile : Profile
    {
        public DateEntryRequestProfile()
        {
            CreateMap<DateEntryCreationRequestDTO, IDateEntry>()
                .ForMember(entry => entry.Id, config => config.MapFrom(_ => string.Empty));
        }
    }
}
