using CountryClubMVC.Extensions;
using DomainServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CountryClubMVC.Controllers
{
    public class UslugeController : Controller
    {
        private readonly IUslugeRepository uslugeRepository;
        private readonly IEnumerable<IValidator<DomainModel.Usluga>> validators;
        public UslugeController(IUslugeRepository uslugeRepository, IEnumerable<IValidator<DomainModel.Usluga>> validators)
        {
            this.uslugeRepository = uslugeRepository;
            this.validators = validators;
        }
        public async Task<IActionResult> Index()
        {
            var data = await uslugeRepository.GetUsluge();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DomainModel.Usluga model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = await uslugeRepository.SaveUsluga(model);
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
            var role = await uslugeRepository.GetUslugaById(id);
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
        public async Task<IActionResult> Edit(DomainModel.Usluga model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    await uslugeRepository.SaveUsluga(model);
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
