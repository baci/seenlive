using AutoMapper;
using SeenLive.Server.DTOs;
using SeenLive.Server.Models;

namespace SeenLive.Server.Profiles
{
    public class DateEntryRequestProfile : Profile
    {
        public DateEntryRequestProfile()
        {
            CreateMap<DateEntryCreationRequestDTO, DateEntry>()
                .ForMember(entry => entry.Id, config => config.MapFrom(_ => string.Empty));
        }
    }
}
