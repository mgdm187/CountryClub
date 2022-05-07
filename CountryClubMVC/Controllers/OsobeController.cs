using AutoMapper;
using CountryClubMVC.Extensions;
using CountryClubMVC.Models;
using DomainModel.Validation;
using DomainServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CountryClubMVC.Controllers
{
    [Authorize(Policy ="Admin")]
    public class OsobeController : Controller
    {
        private readonly IOsobeRepository osobeRepository;
        private readonly IUlogeRepository ulogeRepository;
        private readonly IMjestaRepository mjestaRepository;
        private readonly IClanarineRepository clanarineRepository;
        private readonly IMapper mapper;
        private readonly IEnumerable<IValidator<DomainModel.Osoba>> validators;
        public OsobeController(IOsobeRepository osobeRepository,
                               IEnumerable<IValidator<DomainModel.Osoba>> validators,
                               IUlogeRepository ulogeRepository,
                               IMjestaRepository mjestaRepository,
                               IMapper mapper,
                               IClanarineRepository clanarineRepository)
        {
            this.osobeRepository = osobeRepository;
            this.validators = validators;
            this.ulogeRepository = ulogeRepository;
            this.mjestaRepository = mjestaRepository;
            this.mapper = mapper;
            this.clanarineRepository = clanarineRepository;
        }

        public async Task<IActionResult> Index()
        {
            var data = await osobeRepository.GetOsobe();
            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            var osoba = await osobeRepository.GetOsobaById(id);
            if (osoba == null)
            {
                return NotFound();
            }
            else
            {
                var uloge = await ulogeRepository.GetUloge();
                ViewBag.Uloge = new SelectList(uloge,
                  dataValueField: nameof(DomainModel.Uloga.IdUloga),
                  dataTextField: nameof(DomainModel.Uloga.NazivUloga));
                var clanarine = await clanarineRepository.GetClanarine();
                ViewBag.Clanarine = new SelectList(clanarine,
                  dataValueField: nameof(DomainModel.Clanarina.IdClanarina),
                  dataTextField: nameof(DomainModel.Clanarina.NazivClanarina));
                

                return View(osoba);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OsobaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var domainEntity = mapper.Map<DomainModel.Osoba>(model);
                    domainEntity.Status = (DomainModel.Status)1;
                    domainEntity.IdUloga = 3;
                    await domainEntity.Validate(validators);
                    int personId = await osobeRepository.SaveOsoba(domainEntity);
                    TempData.Put(Constants.ActionStatus, new ActionStatus(true, $"Osoba {model.Ime} {model.Prezime} dodana"));
                    return RedirectToAction(nameof(Details), new { id = personId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var person = await osobeRepository.GetOsobaById(id);
            if (person == null)
            {
                return NotFound();
            }
            else
            {
                OsobaViewModel model = mapper.Map<OsobaViewModel>(person);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OsobaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var osoba = mapper.Map<DomainModel.Osoba>(model);
                    await osoba.Validate(validators);
                    await osobeRepository.SaveOsoba(osoba);
                    return RedirectToAction(nameof(Details), new { id = model.IdOsoba });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public async Task<JsonResult> BlokirajClana(int personId)
        {
            try
            {
                var person = await osobeRepository.GetOsobaById(personId);
                if (person == null)
                {
                    return Json(new ActionStatus(false, $"Invalid person id {personId}"));
                }
                else
                {
                    await osobeRepository.UpdateStatus(personId, 3);
                    return Json(new ActionStatus(true, $"Osoba blokirana!"));
                }
            }
            catch (Exception ex)
            {
                return Json(new ActionStatus(false, ex.CompleteExceptionMessage()));
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await osobeRepository.DeleteOsoba(id);
                TempData.Put(Constants.ActionStatus, new ActionStatus(true, "Osoba obrisana."));
            }
            catch (Exception ex)
            {
                TempData.Put(Constants.ActionStatus, new ActionStatus(false, ex.CompleteExceptionMessage()));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
