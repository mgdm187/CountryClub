using AutoMapper;
using CountryClubMVC.Extensions;
using CountryClubMVC.Models;
using DomainModel;
using DomainModel.Validation;
using DomainServices;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace CountryClubMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOsobeRepository osobeRepository;
        private readonly IUlogeRepository ulogeRepository;
        private readonly IMjestaRepository mjestaRepository;
        private readonly IMapper mapper;
        private readonly IEnumerable<IValidator<DomainModel.Osoba>> validators;
        public AccountController(IOsobeRepository osobeRepository,
                               IEnumerable<IValidator<DomainModel.Osoba>> validators,
                               IUlogeRepository ulogeRepository,
                               IMjestaRepository mjestaRepository,
                               IMapper mapper)
        {
            this.osobeRepository = osobeRepository;
            this.validators = validators;
            this.ulogeRepository = ulogeRepository;
            this.mjestaRepository = mjestaRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registracija(KorisnickiRacunViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var domainEntity = mapper.Map<DomainModel.Osoba>(model);
                    domainEntity.Status = (DomainModel.Status)1;
                    domainEntity.IdUloga = 3;
                    domainEntity.DatumPrijave = DateTime.Now;
                    var mjesto = await mjestaRepository.GetMjestoByPbr(model.Pbr);
                    if(mjesto == null)
                    {
                        mjesto = new DomainModel.Mjesto
                        {
                            NazivMjesto = model.Mjesto,
                            Pbr = model.Pbr
                        };
                        await mjestaRepository.SaveMjesto(mjesto);
                    }
                    domainEntity.IdMjesto = mjesto.IdMjesto.Value;
                    await domainEntity.Validate(validators);
                    int personId = await osobeRepository.SaveOsoba(domainEntity);
                    TempData.Put(Constants.ActionStatus, new ActionStatus(true, $"Uspješno ste se registrirali."));
                    return RedirectToAction("Index", "Home");
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
        public IActionResult Prijava()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Prijava(AccountInfo model)
        {
            var osoba = await osobeRepository.GetOsobaByUsername(model.Username);
            bool isValid = false;
            if(osoba.Username == model.Username && osoba.Lozinka == model.Lozinka && osoba.Status == (Status)1)
            {
                isValid = true;
            }
            if (isValid)
            {
                try
                {
                    var nazivUloge = osoba.nazivUloga;
                    if(nazivUloge == "član")
                    {
                        nazivUloge = "clan";
                    }
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, osoba.Username),
                            new Claim(ClaimTypes.Role, nazivUloge)
                        };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
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

        public async Task<IActionResult> Odjava()
        {
            //
            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MojRacun()
        {
            var osoba = await osobeRepository.GetOsobaByUsername(User.Identity.Name);
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


                return View(osoba);
            }
        }
    }
}
