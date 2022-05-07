using CountryClubMVC.Extensions;
using DomainServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CountryClubMVC.Controllers
{
    public class MjestaController : Controller
    {
        private readonly IMjestaRepository mjestaRepository;
        private readonly IEnumerable<IValidator<DomainModel.Mjesto>> validators;
        public MjestaController(IMjestaRepository mjestaRepository, IEnumerable<IValidator<DomainModel.Mjesto>> validators)
        {
            this.mjestaRepository = mjestaRepository;
            this.validators = validators;
        }
        public async Task<IActionResult> Index()
        {
            var data = await mjestaRepository.GetMjesta();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DomainModel.Mjesto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = await mjestaRepository.SaveMjesto(model);
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
            var role = await mjestaRepository.GetMjestoById(id);
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
        public async Task<IActionResult> Edit(DomainModel.Mjesto model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    await mjestaRepository.SaveMjesto(model);
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
