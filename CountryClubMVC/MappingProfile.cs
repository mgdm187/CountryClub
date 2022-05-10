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

            CreateMap<Rezervacija, ListaRezervacija>()
                .ForMember(dest => dest.IdRezervacija, act => act.MapFrom(src => src.IdRezervacija))
                .ForMember(dest => dest.IdOsoba, act => act.MapFrom(src => src.OsobaId))
                .ForMember(dest => dest.Od, act => act.MapFrom(src => src.DatumPocetka))
                .ForMember(dest => dest.Do, act => act.MapFrom(src => src.DatumZavrsetka))
                .ForMember(dest => dest.Cijena, act => act.MapFrom(src => src.CijenaRezervacije));

            CreateMap<Racun, Infrastructure.EFModel.Racun>()
                .ForMember(dest => dest.Ukupno, act => act.MapFrom(src => src.CijenaUkupno))
                .ForMember(dest => dest.RacunPlacen, act => act.MapFrom(src => src.Placeno));
        }
    }
}
