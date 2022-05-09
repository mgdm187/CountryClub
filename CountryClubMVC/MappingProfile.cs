using AutoMapper;
using CountryClubMVC.Models;
using DomainModel;

namespace CountryClubMVC
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OsobaViewModel, Osoba>()
                .ForMember(dest => dest.NazivMjesto, act => act.MapFrom(src => src.Mjesto));
            CreateMap<Osoba, OsobaViewModel>()
                .ForMember(dest => dest.Mjesto, act => act.MapFrom(src => src.NazivMjesto));

            CreateMap<KorisnickiRacunViewModel, Osoba>()
                .ForMember(dest => dest.NazivMjesto, act => act.MapFrom(src => src.Mjesto))
                .ForMember(dest => dest.Lozinka, act => act.MapFrom(src => src.Lozinka));
            CreateMap<Osoba, KorisnickiRacunViewModel>()
                .ForMember(dest => dest.Mjesto, act => act.MapFrom(src => src.NazivMjesto))
                .ForMember(dest => dest.Lozinka, act => act.MapFrom(src => src.Lozinka));
        }
    }
}
