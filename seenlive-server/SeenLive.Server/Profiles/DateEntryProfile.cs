using AutoMapper;
using SeenLive.Server.DTOs;
using SeenLive.Server.Models;

namespace SeenLive.Server.Profiles
{
    public class DateEntryProfile : Profile
    {
        public DateEntryProfile()
        {
            CreateMap<DateEntry, DateEntryDTO>();
        }
    }    
}
