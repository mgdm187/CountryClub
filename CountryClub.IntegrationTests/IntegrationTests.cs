using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace CountryClub.IntegrationTests
{
    public abstract class IntegrationTests
    {
        protected readonly HttpClient _client;

        public IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Program>();
            _client = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var _content = JsonConvert.SerializeObject(
                new DomainModel.AccountInfo
                {
                    IdOsoba = 0,
                    Username = "mariohorvat",
                    Lozinka = "admin!"
                });

            var buffer = System.Text.Encoding.UTF8.GetBytes(_content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PostAsync("/Account/Prijava", byteContent);

            var registrationResponse = response.Content;
            return null;
        }
    }
}
