using CountryClubMVC.Extensions;
using DomainServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CountryClubMVC.Controllers
{
    public class ClanarineController : Controller
    {
        private readonly IClanarineRepository clanarineRepository;
        private readonly IEnumerable<IValidator<DomainModel.Clanarina>> validators;
        public ClanarineController(IClanarineRepository clanarineRepository, IEnumerable<IValidator<DomainModel.Clanarina>> validators)
        {
            this.clanarineRepository = clanarineRepository;
            this.validators = validators;
        }
        public async Task<IActionResult> Index()
        {
            var data = await clanarineRepository.GetClanarine();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DomainModel.Clanarina model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = await clanarineRepository.SaveClanarina(model);
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
            var role = await clanarineRepository.GetClanarinaById(id);
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
        public async Task<IActionResult> Edit(DomainModel.Clanarina model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    await clanarineRepository.SaveClanarina(model);
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
