using CountryClubMVC.Extensions;
using CountryClubMVC.Models;
using DomainModel;
using DomainServices;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CountryClubMVC.Controllers
{
    public class RacuniController : Controller
    {
        private readonly IOsobeRepository osobeRepository;
        private readonly IUslugeRepository uslugeRepository;
        private readonly IRacuniRepository racuniRepository;
        private readonly IClanarineRepository clanarineRepository;
        //private readonly IEnumerable<IValidator<DomainModel.Rezervacija>> validators;
        private readonly IRezervacijeRepository rezervacijeRepository;
        public RacuniController(IOsobeRepository osobeRepository,
                                IRacuniRepository racuniRepository,
                                IRezervacijeRepository rezervacijeRepository,
                                IClanarineRepository clanarineRepository,
                                IUslugeRepository uslugeRepository)
        {
            this.osobeRepository = osobeRepository;
            this.uslugeRepository = uslugeRepository;
            this.rezervacijeRepository = rezervacijeRepository;
            this.clanarineRepository = clanarineRepository;
            this.racuniRepository = racuniRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await racuniRepository.GetRacuni();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> IndexAccount()
        {
            int idOsoba = (await osobeRepository.GetOsobaByUsername(User.Identity.Name)).IdOsoba.Value;
            var criteria = new SieveModel
            {
                Filters = $"{nameof(SieveCustomFilterMethods.SpecificPerson)}=={idOsoba}"
            };
            var data = await racuniRepository.GetRacuni(criteria);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var racun = await racuniRepository.GetRacun(id);
            if (racun == null)
            {
                return NotFound();
            }
            else
            {
                return View(racun);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PosaljiRacune()
        {
            try
            {
                var listaClanovaSRacunima = await racuniRepository.GetClanoviSRacunima();
                var osobe = await osobeRepository.GetClanoviKlubaId();
                List<Racun> noviRacuni = new List<Racun>();
                foreach (var o in osobe)
                {
                    if (!listaClanovaSRacunima.Contains(o.IdOsoba))
                    {
                        var clanarina = await clanarineRepository.GetClanarinaByDatum(o.DatumRodenja);
                        Racun racun = new DomainModel.Racun
                        {
                            IdOsoba = o.IdOsoba,
                            IdClanarina = clanarina.IdClanarina,
                            CijenaClanarina = clanarina.CijenaClanarina,
                            NazivClanarina = clanarina.NazivClanarina,
                            CijenaUkupno = clanarina.CijenaClanarina,
                            DatumRacuna = DateTime.Now.Date,
                            Ime = o.Ime,
                            Prezime = o.Prezime,
                            Placeno = false
                            
                        };
                        noviRacuni.Add(racun);
                    }
                }
                await racuniRepository.SaveAll(noviRacuni);
                TempData.Put(Constants.ActionStatus, new ActionStatus(true, $"Računi poslani."));
            }
            catch (Exception ex)
            {
                TempData.Put(Constants.ActionStatus, new ActionStatus(false, ex.CompleteExceptionMessage()));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PlacenRacun(int idRacun)
        {
            try
            {
                var racun = await racuniRepository.GetRacun(idRacun);
                if (racun == null)
                {
                    TempData.Put(Constants.ActionStatus, new ActionStatus(false, "Nemoguće dohvatiti račun."));
                }
                else
                {
                    racun.Placeno = true;
                    await racuniRepository.SaveRacun(racun);
                    TempData.Put(Constants.ActionStatus, new ActionStatus(true, $"Račun označen kao plaćen."));
                }
            }
            catch (Exception ex)
            {
                TempData.Put(Constants.ActionStatus, new ActionStatus(false, ex.CompleteExceptionMessage()));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
