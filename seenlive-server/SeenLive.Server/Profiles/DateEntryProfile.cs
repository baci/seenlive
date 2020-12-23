using AutoMapper;
using SeenLive.Server.DTOs;
using SeenLive.DataAccess.Models;

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
