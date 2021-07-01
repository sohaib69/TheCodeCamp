using AutoMapper;
using TheCodeCamp.Models;

namespace TheCodeCamp.Data
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile()
        {
            // ReverseMap will map our CampModel to Camp.
            CreateMap<Camp, CampModel>()
                .ForMember(c => c.VenueName, opt => opt.MapFrom(m => m.Location.VenueName))
                .ReverseMap();

            // ForMember will ignoor few models to be updated.
            CreateMap<Talk, TalksModel>()
                .ReverseMap()
                .ForMember(t => t.Speaker, opt => opt.Ignore())
                .ForMember(t => t.Camp, opt => opt.Ignore());

            CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}