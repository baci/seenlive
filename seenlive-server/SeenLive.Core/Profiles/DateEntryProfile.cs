using AutoMapper;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.DTOs;

namespace SeenLive.Core.Profiles
{
    public class DateEntryProfile : Profile
    {
        public DateEntryProfile()
        {
            CreateMap<IDateEntry, DateEntryDTO>();
        }
    }    
}
