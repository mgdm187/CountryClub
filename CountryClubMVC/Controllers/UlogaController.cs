using CountryClubMVC.Extensions;
using DomainServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using CountryClubMVC.Models;

namespace CountryClubMVC.Controllers
{
    public class UlogaController : Controller
    {
        private readonly IUlogeRepository ulogeRepository;
        private readonly IEnumerable<IValidator<DomainModel.Uloga>> validators;
        public UlogaController(IUlogeRepository ulogeRepository, IEnumerable<IValidator<DomainModel.Uloga>> validators)
        {
            this.ulogeRepository = ulogeRepository;
            this.validators = validators;
        }

        public async Task<IActionResult> Index()
        {
            var data = await ulogeRepository.GetUloge();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DomainModel.Uloga model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = await ulogeRepository.CreateUloga(model.NazivUloga);
                    return RedirectToAction(nameof(Index));
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
            var role = await ulogeRepository.GetUlogaById(id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                return View(role);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DomainModel.Uloga model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    await ulogeRepository.UpdateNazivUloge(model.IdUloga, model.NazivUloga);
                    return RedirectToAction(nameof(Index));
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
    }
}
