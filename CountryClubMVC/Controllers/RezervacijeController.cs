using AutoMapper;
using CountryClubMVC.Extensions;
using CountryClubMVC.Models;
using DomainModel;
using DomainModel.Validation;
using DomainServices;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sieve.Models;

namespace CountryClubMVC.Controllers
{
    public class RezervacijeController : Controller
    {
        private readonly IOsobeRepository osobeRepository;
        private readonly IUslugeRepository uslugeRepository;
        private readonly IRacuniRepository racuniRepository;
        private readonly IMapper mapper;
        private readonly IEnumerable<IValidator<DomainModel.Rezervacija>> validators;
        private readonly IClanarineRepository clanarineRepository;
        private readonly IRezervacijeRepository rezervacijeRepository;
        public RezervacijeController(IOsobeRepository osobeRepository,
                                IRezervacijeRepository rezervacijeRepository,
                                IMapper mapper,
                                IEnumerable<IValidator<DomainModel.Rezervacija>> validators,
                                IClanarineRepository clanarineRepository,
                                IRacuniRepository racuniRepository,
                               IUslugeRepository uslugeRepository)
        {
            this.osobeRepository = osobeRepository;
            this.uslugeRepository = uslugeRepository;
            this.mapper = mapper;
            this.validators = validators;
            this.rezervacijeRepository = rezervacijeRepository;
            this.racuniRepository = racuniRepository;
            this.clanarineRepository = clanarineRepository;
        }

        //[Authorize("Admin")]
        public async Task<IActionResult> Index()
        {
            var data = await rezervacijeRepository.GetRezervacije();
            return View(data);
        }

        public async Task<IActionResult> IndexAccount()
        {
            var idOsoba = (await osobeRepository.GetOsobaByUsername(User.Identity.Name)).IdOsoba.Value;
            var data = await rezervacijeRepository.GetRezervacije(idOsoba);
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rezervacija = await rezervacijeRepository.GetRezervacija(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            else
            {
                return View(rezervacija);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            var usluge = await uslugeRepository.GetUsluge();
            ViewBag.Usluge = new SelectList(usluge,
              dataValueField: nameof(DomainModel.Usluga.IdUsluga),
              dataTextField: nameof(DomainModel.Usluga.NazivUsluga));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RezViewModel data)
        {
                try
                {
                Osoba osoba = (await osobeRepository.GetOsobaByUsername(User.Identity.Name));
                DateTime datum = Convert.ToDateTime(data.Datum).Date;
                List<RezerviranaUsluga> usluge = new List<RezerviranaUsluga>();
                var minPocetak = datum.AddDays(1);
                var maxZavrsetak = datum;
                decimal cijenaRezervacije = 0.00M;
                var criteria = new SieveModel
                {
                    Filters = $"{nameof(SieveCustomFilterMethods.RacunPostoji)}=={osoba.IdOsoba.Value}"
                };
                var racun = await racuniRepository.GetTekuciRacun(criteria);
                foreach (var usluga in data.Usluge)
                {
                    var uslugaId = Convert.ToInt32(usluga.Usluga);
                    var poc = Convert.ToDateTime(datum.Date.ToString().Split(' ')[0] + Convert.ToDateTime(usluga.Pocetak).TimeOfDay.ToString());
                    var zav = Convert.ToDateTime(datum.Date.ToString().Split(' ')[0] + Convert.ToDateTime(usluga.Zavrsetak).TimeOfDay.ToString());
                    var u = new RezerviranaUsluga
                    {
                        IdUsluga = uslugaId,
                        Od = poc,
                        Do = zav,
                        ProvedenoVrijeme = (zav.Subtract(poc)).Hours,
                        Cijena = (zav.Subtract(poc)).Hours * (await uslugeRepository.GetUslugaById(uslugaId)).CijenaUsluga
                    };
                    if(poc < minPocetak)
                    {
                        minPocetak = poc;
                    }
                    if(zav > maxZavrsetak)
                    {
                        maxZavrsetak = zav;
                    }
                    cijenaRezervacije += u.Cijena;
                    usluge.Add(u);
                }
                Rezervacija model = new Rezervacija
                {
                    DatumKreiranja = DateTime.Now,
                    DatumPocetka = minPocetak,
                    DatumZavrsetka = maxZavrsetak,
                    Usluge = usluge,
                    CijenaRezervacije = cijenaRezervacije,
                    OsobaId = osoba.IdOsoba.Value,
                    IdRacun = racun.IdRacun.Value
                };
                await model.Validate(validators);
                int rezervacijaId = await rezervacijeRepository.SaveRezervacija(model);
                if(racun != null)
                {
                    await racuniRepository.UpdateCijenaRacuna(racun.IdRacun.Value, model.CijenaRezervacije);
                }
                else
                {
                    List<ListaRezervacija> listaRezervacija = new List<ListaRezervacija>();
                    var rezervacija = mapper.Map<DomainModel.ListaRezervacija>(model);
                    listaRezervacija.Add(rezervacija);
                    racun.Rezervacije = listaRezervacija;
                    racun.IdOsoba = osoba.IdOsoba.Value;
                    var clanarina = await clanarineRepository.GetClanarinaByDatum(osoba.DatumRodenja);
                    racun.IdClanarina = clanarina.IdClanarina.Value;
                    racun.NazivClanarina = clanarina.NazivClanarina;
                    racun.CijenaClanarina = clanarina.CijenaClanarina;
                    int id = await racuniRepository.SaveRacun(racun);
                }
                TempData.Put(Constants.ActionStatus, new ActionStatus(true, $"Rezervacija kreirana"));
                    //return RedirectToAction(nameof(Details), new { id = rezervacijaId });
                    return Json(new ActionStatus(true, "Rezervacija kreirana"));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.CompleteExceptionMessage());
                return Json(new ActionStatus(false, ex.CompleteExceptionMessage()));
                //return RedirectToAction(nameof(IndexAccount));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                bool result = await rezervacijeRepository.DeleteRezervacija(id);
                if (result)
                {
                    TempData.Put(Constants.ActionStatus, new ActionStatus(true, "Rezervacija obrisana."));
                }
                else
                {
                    TempData.Put(Constants.ActionStatus, new ActionStatus(false, "Ne možete obrisati rezervaciju."));
                }
            }
            catch (Exception ex)
            {
                TempData.Put(Constants.ActionStatus, new ActionStatus(false, ex.CompleteExceptionMessage()));
            }
            return RedirectToAction(nameof(IndexAccount));
        }
    }
}
