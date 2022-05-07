using AutoMapper;
using DomainModel.Validation;
using DomainServices;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.EFModel;
using Infrastructure.ValidationRequestHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using CountryClubMVC;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<OsobaValidator>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Home/Index";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("Role", "admin"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClanKluba", policy =>
        policy.RequireClaim("Role", "clan"));
});


#region Setup dependencies
builder.Services.AddDbContext<CountryclubContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Project")));
builder.Services.AddTransient<IUlogeRepository, UlogeRepository>();
builder.Services.AddTransient<IClanarineRepository, ClanarineRepository>();
builder.Services.AddTransient<IMjestaRepository, MjestaRepository>();
builder.Services.AddTransient<IUslugeRepository, UslugeRepository>();
builder.Services.AddTransient<IOsobeRepository, OsobeRepository>();

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));
builder.Services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
builder.Services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();



builder.Services.AddMediatR(typeof(CheckNazivUlogeRequestHandler));
builder.Services.AddMediatR(typeof(CheckNazivClanarineRequestHandler));
builder.Services.AddMediatR(typeof(CheckPbrRequestHandler));
builder.Services.AddMediatR(typeof(CheckNazivUslugeRequestHandler));
#endregion

#region AutoMapper settings
Action<IServiceProvider, IMapperConfigurationExpression> mapperConfigAction = (serviceProvider, cfg) =>
{
    cfg.ConstructServicesUsing(serviceProvider.GetService);
};
builder.Services.AddAutoMapper(mapperConfigAction, typeof(MappingProfile)); //assemblies containing mapping profiles            
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapDefaultControllerRoute();

app.Run();
